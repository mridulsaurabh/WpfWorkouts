using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ExportManager
{
    public class ExportHelper
    {
        #region enums and constants
        private const int TOTAL_AVAILABLE_ROWS_IN_A_WORKSHEET = 1048576; // EXCEL 2007/10 SPECIFICATIONS 
        private const int ROWS_USED_TO_ADD_FILE_HEADER = 15;       // ROWS USED TO ADD CUSTOM FILE HEADER.
        private const int ROWS_TO_BE_ADDED_IN_ONE_SHOT = 100000;   // DATA ROWS FOR WRITING IN EXCEL IN ONE SHOT AS MULTIPLE OF 1 LAKH ( BEING USED TO OPTIMIZE THE PERFORMANCE) 
        private const int TOTAL_ROWS_ADDED_IN_WORKBOOK = 10000000; // DATA ROWS TO BE ADDED IN EXPORTED FILE ( DOESN'T INCLUDE FILE HEADER )
        #endregion

        #region fields and constructors
        private Excel._Application m_ExApp = null;
        private Excel.Workbooks m_WorkBooks = null;
        private Excel.Workbook m_WorkBook = null;
        private Excel.Worksheets m_ExcelSheets = null;
        private Excel.Worksheet m_ExcelSheet = null;
        private Excel.Range m_ExcelRange = null;
        private object[,] m_ExcelData = null;
        private ExportDataType m_Type;
        private string filePath = string.Empty;
        private int m_CurrentWorkSheetInUse = 0;
        private int m_RowCountInCurrentSheet = 0;
        private ConcurrentQueue<Object> dataValuesInQueue = new ConcurrentQueue<object>();
        private ManualResetEvent m_MaunalEvent = new ManualResetEvent(false);
        private bool HasAllRowAdded = false;
        private Stopwatch watchDog = new Stopwatch();
        private int m_TotalRowsNeeded;

        public ExportHelper(ExportDataType type, string exportFilePath, int numberOfRowsNeeded)
        {
            m_Type = type;
            m_TotalRowsNeeded = numberOfRowsNeeded;
            filePath = exportFilePath;
            InitializeExcelApplication();
            AddCustomFileHeader();
            AddDataRowHeader(ROWS_USED_TO_ADD_FILE_HEADER);
        }

        #endregion

        #region properties and delegates

        public List<object> Headers
        {
            get
            {
                List<object> retValues = null;
                if (m_Type != ExportDataType.None)
                {
                    switch (m_Type)
                    {
                        case ExportDataType.PressureSensor:
                            retValues = new List<object>() { "Date & Time", "Run Time", "Value P", "Notes", "Event Details" };
                            break;
                        case ExportDataType.TemperatureConductivitySensor:
                            retValues = new List<object>() { "Date & Time", "Run Time", "Value C", "Value T", "Notes", "Event Details" };
                            break;
                        case ExportDataType.Experiment:
                            retValues = new List<object>() { "Date & Time", "Run Time", "Cal Value", "Value 1", "Value 2", "Value 3", "Notes", "Event Details" };
                            break;
                        default:
                            break;
                    }
                }
                return retValues;
            }
        }

        public ExportFormatExtention ExportFormat
        {
            get
            {
                ExportFormatExtention format = ExportFormatExtention.None;
                string extension = Path.GetExtension(filePath);
                if (extension.Contains("xls"))
                {
                    format = ExportFormatExtention.Excel;
                }
                else if (extension.Contains("pdf"))
                {
                    format = ExportFormatExtention.PDF;
                }

                return format;
            }
        }

        #endregion

        #region events and methods

        private void InitializeExcelApplication()
        {
            m_ExApp = new Excel.Application();
            m_ExApp.Visible = false;
            m_WorkBooks = (Excel.Workbooks)m_ExApp.Workbooks;
            m_WorkBook = (Excel.Workbook)m_WorkBooks.Add(Missing.Value);
            m_ExcelSheets = (Excel.Worksheets)m_WorkBook.Worksheets;
            m_ExcelSheet = (Excel.Worksheet)(m_ExcelSheets.get_Item(++m_CurrentWorkSheetInUse));
            ApplyStyleToExcelWorkbook();
        }

        public void ExportDataInToFile()
        {
            Thread readThread = new Thread(new ThreadStart(ReadAndEnqueueData));
            Thread writeThread = new Thread(new ThreadStart(WriteAndDequeueData));
            readThread.Start();
            writeThread.Start();
            watchDog.Start();
        }

        private void AddCustomFileHeader()
        {
            m_RowCountInCurrentSheet = ROWS_USED_TO_ADD_FILE_HEADER;
            FreezeRows(ROWS_USED_TO_ADD_FILE_HEADER);
            InsertImageAndMergeCells("A1", "B2");
            if (m_Type == ExportDataType.PressureSensor || m_Type == ExportDataType.TemperatureConductivitySensor)
            {
                AddSensorExportFileHeader();
            }
            else if (m_Type == ExportDataType.Experiment)
            {
                AddExperimentExportFileHeader();
            }
            m_ExcelSheet.Range["A1", "I1"].EntireColumn.AutoFit();
        }

        private void ReadAndEnqueueData()
        {
            int counter = 0;
            while (counter != m_TotalRowsNeeded)
            {
                switch (m_Type)
                {
                    case ExportDataType.PressureSensor:
                        dataValuesInQueue.Enqueue(new ExportData(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "00:00:00", counter, "first Notification added", ""));
                        break;
                    case ExportDataType.TemperatureConductivitySensor:
                        dataValuesInQueue.Enqueue(new ExportData(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "00:00:00", counter, 5, "first Notification added", ""));
                        break;
                    case ExportDataType.Experiment:
                        dataValuesInQueue.Enqueue(new ExportData(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "00:00:00", counter, new List<int>() { 11, 23, 45 }, "first Notification added", ""));
                        break;
                    default:
                        break;
                }
                counter++;
                m_MaunalEvent.Set();
            }
            HasAllRowAdded = true;
        }

        private void WriteAndDequeueData()
        {
            int exWriteCounter = 0;
            int startRowIdentifier = ROWS_USED_TO_ADD_FILE_HEADER + 1;
            string startColumn = "A";
            List<object> dataRowsToBeAddedInOneShot = new List<object>();

            while (m_MaunalEvent.WaitOne())
            {
                while (dataValuesInQueue.Count > 0)
                {
                    object cData;
                    if (dataValuesInQueue.TryDequeue(out cData))
                    {
                        dataRowsToBeAddedInOneShot.Add(cData);
                        m_RowCountInCurrentSheet++;
                        if (m_RowCountInCurrentSheet >= TOTAL_AVAILABLE_ROWS_IN_A_WORKSHEET)
                        {
                            int startRow = startRowIdentifier + exWriteCounter * ROWS_TO_BE_ADDED_IN_ONE_SHOT;
                            string exRange = startColumn + startRow;
                            WriteDataInToExportFile(dataRowsToBeAddedInOneShot, dataRowsToBeAddedInOneShot.Count, Headers.Count, exRange);

                            if (dataValuesInQueue.Count > 0)
                            {
                                // CLEAR ALL THE FLAGS, COUNTERS AND COLLECTION MAINTAINED FOR A WORKSHEET.
                                exWriteCounter = 0;
                                startRowIdentifier = 2; //1ST ROW IS BEING USED TO ADD ROW HEADER IN EACH WORKSHEET.
                                m_RowCountInCurrentSheet = 0;
                                dataRowsToBeAddedInOneShot.Clear();

                                // CREATING NEW TAB IN TO WORKBOOK FOR FURTHER WRITING ON DATA IN TO EXCEL.
                                if (m_WorkBook.Sheets.Count <= m_CurrentWorkSheetInUse)
                                {
                                    m_WorkBook.Sheets.Add(Type.Missing, m_WorkBook.Sheets[m_WorkBook.Sheets.Count], Type.Missing, Type.Missing);
                                    m_ExcelSheet = (Excel.Worksheet)(m_ExcelSheets.get_Item(++m_CurrentWorkSheetInUse));
                                    AddDataRowHeader(1);
                                    m_RowCountInCurrentSheet++;
                                }
                            }
                        }
                        if (dataRowsToBeAddedInOneShot.Count == ROWS_TO_BE_ADDED_IN_ONE_SHOT)
                        {
                            int startRow = startRowIdentifier + exWriteCounter * ROWS_TO_BE_ADDED_IN_ONE_SHOT;
                            string exRange = startColumn + startRow;
                            WriteDataInToExportFile(dataRowsToBeAddedInOneShot, dataRowsToBeAddedInOneShot.Count, Headers.Count, exRange);

                            dataRowsToBeAddedInOneShot.Clear();
                            exWriteCounter++;
                        }
                        if (HasAllRowAdded)
                        {
                            if (dataRowsToBeAddedInOneShot.Count > 0)
                            {
                                // THIS IS TO WRITE LEFT OVER DATA WHICH IS NOT MULTIPLE TO DATA_ROWS_TO_BE_ADDED_IN_ONE_SHOT
                                int startRow = startRowIdentifier + exWriteCounter * ROWS_TO_BE_ADDED_IN_ONE_SHOT;
                                string exRange = startColumn + startRow;
                                WriteDataInToExportFile(dataRowsToBeAddedInOneShot, dataRowsToBeAddedInOneShot.Count, Headers.Count, exRange);
                            }
                            if (m_WorkBook.Sheets.Count <= m_CurrentWorkSheetInUse)
                            {
                                m_WorkBook.Sheets.Add(Type.Missing, m_WorkBook.Sheets[m_WorkBook.Sheets.Count], Type.Missing, Type.Missing);
                                m_ExcelSheet = (Excel.Worksheet)(m_ExcelSheets.get_Item(++m_CurrentWorkSheetInUse));
                                m_ExcelSheet.Name = "Graph";
                                string fPath = Directory.GetCurrentDirectory() + "\\Images\\Chart.png";
                                //m_ExcelSheet.Shapes.AddPicture(fPath,
                                //                          Microsoft.Office.Core.MsoTriState.msoFalse,
                                //                          Microsoft.Office.Core.MsoTriState.msoCTrue,
                                //                          10, 10, 650, 300);

                                SaveAndCloseExcelWorkBook();
                            }
                            m_MaunalEvent.Reset();
                        }
                    }
                }
            }
        }

        private void WriteDataInToExportFile(List<object> dataValues, int rowConunt, int columnCount, string exRange)
        {
            FillRowData(dataValues, rowConunt, columnCount);
            m_ExcelRange = m_ExcelSheet.get_Range(exRange, Missing.Value);
            m_ExcelRange = m_ExcelRange.get_Resize(rowConunt, columnCount);
            m_ExcelRange.set_Value(Missing.Value, m_ExcelData);
            ApplyBorder(m_ExcelRange);
            m_ExcelRange.EntireColumn.AutoFit();
        }

        private void AddExperimentExportFileHeader()
        {

        }

        private void AddSensorExportFileHeader()
        {

        }

        private void FillRowData(List<object> dataValues, int rowConunt, int columnCount)
        {
            m_ExcelData = new object[rowConunt, columnCount];
            for (int row = 0; row < rowConunt - 1; row++)
            {
                if (dataValues[row] != null)
                {
                    FillIndividualRow(dataValues[row], row);
                }
                else
                {
                    // YOU CAN'T REACH HERE.....IT MEANS SOMETHING WENT WRONG WHILE ADDING/REMOVING DATA FROM THE QUEUE.
                    // DATA ROW VALUES CANT BE NULL.
                }
            }
        }

        private void FillIndividualRow(object obj, int row)
        {
            ExportData currentData = obj as ExportData;
            switch (currentData.ExportType)
            {
                case ExportDataType.PressureSensor:
                    m_ExcelData[row, 0] = currentData.DateAndTime;
                    m_ExcelData[row, 1] = currentData.RunTime;
                    m_ExcelData[row, 2] = currentData.PresentValue;
                    m_ExcelData[row, 3] = currentData.Notification;
                    m_ExcelData[row, 4] = currentData.EventDetails;
                    break;
                case ExportDataType.TemperatureConductivitySensor:
                    m_ExcelData[row, 0] = currentData.DateAndTime;
                    m_ExcelData[row, 1] = currentData.RunTime;
                    m_ExcelData[row, 2] = currentData.PresentValue;
                    m_ExcelData[row, 3] = currentData.ReferenceValue;
                    m_ExcelData[row, 4] = currentData.Notification;
                    m_ExcelData[row, 5] = currentData.EventDetails;
                    break;
                case ExportDataType.Experiment:
                    int count = currentData.AttachedSensorValues.Count;
                    m_ExcelData[row, 0] = currentData.DateAndTime;
                    m_ExcelData[row, 1] = currentData.RunTime;
                    m_ExcelData[row, 2] = currentData.PresentValue;
                    for (int i = 1; i <= count; i++)
                    {
                        m_ExcelData[row, 2 + i] = currentData.AttachedSensorValues[i - 1];
                    }
                    m_ExcelData[row, 2 + count + 1] = currentData.Notification;
                    m_ExcelData[row, 2 + count + 2] = currentData.EventDetails;
                    break;
                default:
                    break;
            }
        }

        private void AddDataRowHeader(int dataRow)
        {
            string startColumn = "A" + dataRow;
            string endColumn = ExcelColumnFromNumber(Headers.Count) + dataRow;
            var headerArray = (from item in Headers select item as object).ToArray();
            m_ExcelRange = m_ExcelSheet.get_Range(startColumn, endColumn);
            m_ExcelRange.set_Value(Missing.Value, headerArray);
            ApplyHeaderStyle(m_ExcelRange);
        }

        private string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        private void ApplyStyleToExcelWorkbook()
        {
            Excel.Style style = m_WorkBook.Styles.Add("NewStyle");
            style.Font.Name = "Calibri";
            style.Font.Size = 12;
            style.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            style.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            Excel.Range appRange = m_ExApp.get_Range("A1", "J1048576");
            appRange.Style = "NewStyle";
        }

        private void ApplyHeaderStyle(Range range, bool isHeaderTextBold = true)
        {
            if (isHeaderTextBold)
            {
                range.Font.Bold = true;
            }
            //  FILL COLOR FOR THE HEADER CELLS, APPLY BORDER, SET AUTO FIT
            range.Cells.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(220, 230, 241));
            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.Weight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            range.EntireColumn.AutoFit();
        }

        private void ApplyBorder(Range range)
        {
            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.Weight = Excel.XlBorderWeight.xlThin;
            range.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        }

        private void FreezeRows(int numberOfRows)
        {
            m_ExcelSheet.Activate();
            m_ExcelSheet.Application.ActiveWindow.SplitRow = numberOfRows;
            m_ExcelSheet.Application.ActiveWindow.FreezePanes = true;

            //APPLY FILTER

        }

        private void InsertImageAndMergeCells(string sRow, string eRow)
        {
            Excel.Range oRange = m_ExcelSheet.Range[sRow, eRow];
            float left = (float)((double)oRange.Left);
            float top = (float)((double)oRange.Top);
            string fPath = Directory.GetCurrentDirectory() + "\\Images\\logo.png";
            // m_ExcelSheet.Shapes.AddPicture(fPath,

            m_ExcelSheet.Range[sRow, eRow].Merge();
        }

        private void SaveAndCloseExcelWorkBook()
        {
            try
            {
                //  SET ALL THE SHEET PAGE SET ORIENTATION TO LANDSCAPE TO MAKE IT PRINTABLE.
                foreach (_Worksheet cSheet in m_WorkBook.Sheets)
                {
                    cSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                    cSheet.PageSetup.RightHeader = "BioLambda";
                    cSheet.PageSetup.LeftFooter = filePath;
                    cSheet.PageSetup.RightFooter = "PageNumber";
                }

                if (ExportFormat == ExportFormatExtention.Excel)
                {
                    m_WorkBook.SaveAs(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing);
                }
                else if (ExportFormat == ExportFormatExtention.PDF)
                {
                    m_WorkBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, filePath,
                        XlFixedFormatQuality.xlQualityMinimum, true, false, Missing.Value, Missing.Value, false, Missing.Value);
                }
                Trace.Write("Data has been successfully exported to the file.");
                TimeSpan totalTimeTakenToCreateFile = watchDog.Elapsed;
                Trace.Write(string.Format("Total time taken to create the file : {0}", totalTimeTakenToCreateFile.ToString()));
                watchDog.Reset();
            }
            catch (Exception)
            {
                Trace.Write("Error occured while saving export file.");
                throw;
            }
            finally
            {
                int processId = 0;
                NativeMethods.GetWindowThreadProcessId(m_ExApp.Hwnd, out processId);
                if (m_WorkBook != null)
                {
                    m_WorkBook.Close(false, Type.Missing, Type.Missing);
                    m_WorkBook = null;
                }
                if (m_ExApp != null)
                {
                    m_ExApp.Quit();
                    Marshal.ReleaseComObject(m_ExApp);
                    m_ExApp = null;
                }
                CheckExcelProcessAndKill(processId);
            }
        }

        public static void CheckExcelProcessAndKill(int processId)
        {
            Process[] processes = Process.GetProcessesByName("EXCEL");
            var excelProcess = from s in processes
                               where s.Id == processId
                               select s;
            Process process = excelProcess.Single();
            process.Kill();
        }

        #endregion
    }

    class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(int hwnd, out int lpdwPocessId);
    }


    public enum ExportDataType
    {
        None,
        PressureSensor,
        TemperatureConductivitySensor,
        Experiment
    }

    public enum ExportFormatExtention
    {
        None,
        Excel,
        PDF
    }

}
