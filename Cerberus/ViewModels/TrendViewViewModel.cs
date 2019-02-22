using ExportManager;
using Infrastructure.Commands;
using Infrastructure.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;

namespace Cerberus
{
    public class TrendViewViewModel : BaseViewModel
    {
        #region fields and constructors
        private string _selectedTrend;
        private TrendType _currentType = TrendType.None;
        private ExportDataType _currentDataType = ExportDataType.None;
        private string _filePath = string.Empty;

        public TrendViewViewModel(IWorkAsync service, IEventAggregator eventAggregator)
            : base(service, eventAggregator)
        {
            //this._currentType = selectedType;
            //this._selectedTrend = sTrend;
            this.AddNote = new AwaitableDelegateCommand<object[]>(OnNotesBeingAdded);
            this.ExportCommand = new AwaitableDelegateCommand<string>(OnSelectedTrendExported);
        }
        #endregion

        #region properties and delegates

        public string SelectedTrend
        {
            get
            {
                return _selectedTrend;
            }
            set
            {
                SetProperty(ref _selectedTrend, value);
                OnSelectedTrendChanged();
            }
        }

        public List<string> AvailableModules
        {
            get
            {
                var availableModules = new List<string>();
                switch (_currentType)
                {
                    case TrendType.None:
                        break;
                    case TrendType.SensorModule:
                        foreach (var module in SensorModules)
                        {
                            availableModules.Add(module.Name);
                        }
                        break;
                    case TrendType.Experiment:
                        foreach (var module in AvailableExperiments)
                        {
                            availableModules.Add(module.Name);
                        }
                        break;
                    default:
                        break;
                }
                return availableModules;
            }
        }

        public ICommand AddNote
        {
            get;
            set;
        }

        public ICommand ExportCommand
        {
            get;
            set;
        }

        #endregion

        #region events and methods

        private void OnSelectedTrendChanged()
        {
            // CLEAR AND UPDATE THE CONTENT OF THE CHART CONTROL AS PER THE SELECTED TREND. 
            Notifier.Instance.Notify("Application", string.Format("Data is unavailable for the selected trend : {0}.", _selectedTrend), Category.Error);

        }

        private void OnNotesBeingAdded(object[] parameters)
        {
            string addedNote = parameters[0].ToString();
            Notifier.Instance.Notify("Application", "Note has been added into the log file.", Category.Information);
        }

        private void OnSelectedTrendExported(string selectedTrend)
        {
            GetSelectedTrendModuleType(selectedTrend);
            if (_currentDataType != ExportDataType.None)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Excel 2007 Files(.xls)|*.xls|Excel 2010 Files(.xlsx)|*.xlsx| PDF Files(*.pdf)|*.pdf";
                dialog.FilterIndex = 1;
                bool? userClickedOK = dialog.ShowDialog();
                if (userClickedOK == true && !string.IsNullOrEmpty(dialog.FileName))
                {
                    _filePath = dialog.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (s, e) =>
                        {
                            IsBusy = true;
                            BusyMessage = "Please wait while data is being exported for selected trend.";
                            //ExportHelper exHelper = new ExportHelper(currentDataType, filePath, 1000000);
                            Thread.Sleep(5000);
                            //exHelper.ExportDataInToFile();
                        };
                    worker.RunWorkerAsync();
                    worker.RunWorkerCompleted += OnExportRunWorkerCompleted;
                }
            }
            Notifier.Instance.Notify("Application", "Available data for the selected trend could not be exported.", Category.Error);
        }

        private void OnExportRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
        }

        private void GetSelectedTrendModuleType(string selectedTrend)
        {
            if (this._currentType == TrendType.SensorModule)
            {
                var currentTrend = this.SensorModules.FindMatch(t => t.Name == selectedTrend);
                if (currentTrend != null)
                {
                    _currentDataType = (currentTrend.Type == ModuleType.PressureModule) ? ExportDataType.PressureSensor
                                                                        : ExportDataType.TemperatureConductivitySensor;
                }
            }
            else
            {
                _currentDataType = ExportDataType.Experiment;
            }
        }

        #endregion
    }
}
