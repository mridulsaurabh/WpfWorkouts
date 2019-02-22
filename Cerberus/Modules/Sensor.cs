using System;
using System.ComponentModel;
using System.Collections.Generic;
using Infrastructure.Common;

namespace Cerberus
{
    public class Sensor : ModelBase
    {
        #region fields and constructors
        private string m_ID;
        private string m_Unit;
        private SensorType m_Type;    
        private Alarm m_AlarmValues;
        private double m_PresentValue;     

        public Sensor(SensorType type)
        {
            m_Type = type;
        }
        #endregion

        #region properties and deleagtes 

        public string ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                SetProperty(ref this.m_ID, value);
            }
        }

        public string Reader
        {
            get;
            set;
        }

        public string Unit
        {
            get
            {
                return m_Unit;
            }
            set
            {
                SetProperty(ref this.m_Unit, value);   
                OnPropertyChanged("DisplayUnit");                
            }
        }

        public SensorType Type
        {
            get
            {
                return m_Type;
            }
        }

        public Alarm AlarmValues
        {
            get
            {
                return m_AlarmValues;
            }
            set
            {
                SetProperty(ref this.m_AlarmValues, value);
            }
        }             

        public double PresentValue
        {
            get
            {
                return m_PresentValue;
            }
            set
            {
                SetProperty(ref this.m_PresentValue, value);
            }
        }

        public string DisplayUnit
        {
            get
            {
                return string.Format("{0} ", m_Unit);
            }
        }               

        public List<string> AvailableUnits
        {
            get
            {
                return UnitTable.GetAvailableUnits(m_Type);
            }
        }        

        #endregion
    }

    public class Alarm
    {
        #region fields and constructors
        public Alarm(double low, double high)
        {
            Low = low;
            High = high;
        }

        #endregion

        #region properties and delegates

        public double Low
        {
            get;
            set;
        }

        public double High
        {
            get;
            set;
        }

        public double LowLow
        {
            get;
            set;
        }

        public double HighHigh
        {
            get;
            set;
        }

        #endregion
    }

}
