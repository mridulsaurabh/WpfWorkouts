using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager
{
    public class ExportData
    {
        public ExportData(string dateTime, string runTime, int presentValue, string notification, string eventDetails)
        {
            this.DateAndTime = dateTime;
            this.RunTime = runTime;
            this.PresentValue = presentValue;
            this.Notification = notification;
            this.EventDetails = eventDetails;
        }

        public ExportData(string dateTime, string runTime, int pValue, int refValue, string notification, string eventDetails)
        {
            this.DateAndTime = dateTime;
            this.RunTime = runTime;
            this.PresentValue = pValue;
            this.ReferenceValue = refValue;
            this.Notification = notification;
            this.EventDetails = eventDetails;
        }

        public ExportData(string dateTime, string runTime, int presentValue, List<int> attachedSensorValues, string notification, string eventDetails)
        {
            this.DateAndTime = dateTime;
            this.RunTime = runTime;
            this.PresentValue = presentValue;
            this.AttachedSensorValues = attachedSensorValues;
            this.Notification = notification;
            this.EventDetails = eventDetails;
        }

        public ExportDataType ExportType { get; set; }

        public string DateAndTime { get; set; }

        public string RunTime { get; set; }

        public double PresentValue { get; set; }

        public string Notification { get; set; }

        public string EventDetails { get; set; }

        public double ReferenceValue { get; set; }

        public List<int> AttachedSensorValues { get; set; }
    }
}
