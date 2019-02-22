using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Infrastructure
{
    public class Logger
    {
        private TraceSource _trace;
        private volatile static Logger _logger = null;
        private static object _syncToken = new object();
        private const string _logfileName = "Cerberus.log";
        private Logger()
        {
            _trace = new TraceSource("Cerberus");
            string logfilePath = AppDomain.CurrentDomain.BaseDirectory + _logfileName;
            TextWriterTraceListener cListener = new TextWriterTraceListener(logfilePath);
            Trace.Listeners.Clear();
            Trace.Listeners.Add(cListener);
        }

        public static Logger Instance
        {
            get
            {
                lock (_syncToken)
                {
                    if (_logger == null)
                    {
                        _logger = new Logger();
                    }
                    return _logger;
                }
            }
        }

        public string LogFile { get; set; }

        public void LogException(Exception ex)
        {
            try
            {
                StringBuilder exBuilder = new StringBuilder();
                exBuilder.Append("---------------------------------------------------------------------");
                exBuilder.AppendLine();
                exBuilder.Append(DateTime.Now.ToString());
                exBuilder.AppendLine();
                exBuilder.Append(ex.Message);
                exBuilder.AppendLine();
                exBuilder.Append(ex.Source);
                exBuilder.AppendLine();
                exBuilder.Append(ex.StackTrace);
                exBuilder.AppendLine();
                if (ex.InnerException != null)
                    exBuilder.Append(ex.InnerException.Message);
                Trace.TraceError(exBuilder.ToString());

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Trace.Flush();
            }
        }

        public void LogInformation(string information)
        {
            try
            {
                StringBuilder exBuilder = new StringBuilder();
                exBuilder.Append("---------------------------------------------------------------------");
                exBuilder.AppendLine();
                exBuilder.Append(DateTime.Now.ToString());
                exBuilder.AppendLine();
                exBuilder.Append(information);
                Trace.TraceInformation(exBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Trace.Flush();
            }
        }

    }
}
