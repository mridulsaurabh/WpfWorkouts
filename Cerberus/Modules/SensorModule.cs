using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Infrastructure.Common;
using System.ComponentModel.DataAnnotations;

namespace Cerberus
{
    public class SensorModule : ModelBase
    {
        #region fields and constructors
        private string m_Name;
        private ModuleType m_Type;
        private string m_ParentReader;
        private bool m_IsActivated;
        private ModuleParameterStatus m_CurrentModuleParameter;
        private List<Sensor> m_AttachedSensorToThisModule;
        private DateTime m_DateAndTime;
        private bool m_Marked = false;
        private LogFile m_LogFile;
        private bool m_HasNotifications = false;
        private string _phoneNumber;

        public SensorModule(ModuleType type, string reader, List<Sensor> addedSensorsToThisModule)
        {
            m_Type = type;
            m_ParentReader = reader;
            m_LogFile = new LogFile();
            m_CurrentModuleParameter = ModuleParameterStatus.All;
            m_AttachedSensorToThisModule = addedSensorsToThisModule;
        }
        #endregion

        #region properties and delegates

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Sensor Name can not be empty.")]
        [MaxLength(12, ErrorMessage = "Sensor name can not be longer than 12 charectors")]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                SetProperty(ref this.m_Name, value);
                ValidateProperty(value);
            }
        }

        public ModuleType Type
        {
            get
            {
                return m_Type;
            }
        }

        public string ParentReader
        {
            get
            {
                return m_ParentReader;
            }
            set
            {
                SetProperty(ref this.m_ParentReader, value);
            }
        }

        public bool IsActivated
        {
            get
            {
                return m_IsActivated;
            }
            set
            {
                SetProperty(ref this.m_IsActivated, value);
            }
        }

        public bool HasNotifications
        {
            get
            {
                return m_HasNotifications;
            }
            set
            {
                SetProperty(ref this.m_HasNotifications, value);
            }
        }

        public ModuleParameterStatus CurrentModuleParameter
        {
            get
            {
                return m_CurrentModuleParameter;
            }
            set
            {
                SetProperty(ref this.m_CurrentModuleParameter, value);
                OnPropertyChanged(() => this.ModuleSensors);
            }
        }

        public List<Sensor> ModuleSensors
        {
            get
            {
                return InitializeSensorsAddedToModule(m_CurrentModuleParameter, m_AttachedSensorToThisModule);
            }
        }

        public bool IsMarked
        {
            get
            {
                return m_Marked;
            }
            set
            {
                SetProperty(ref this.m_Marked, value);
            }
        }

        public DateTime DateAndTime
        {
            get
            {
                return m_DateAndTime;
            }
            set
            {
                SetProperty(ref this.m_DateAndTime, value);
            }
        }

        public LogFile LogFile
        {
            get
            {
                return m_LogFile;
            }
            set
            {
                SetProperty(ref this.m_LogFile, value);
            }
        }


        [CustomValidation(typeof(SensorModule), "CheckAcceptableAreaCodes")]
        [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "You must enter a 10 digit phone number in a US format")]
        public string Phone
        {
            get { return _phoneNumber; }
            set 
            { 
                SetProperty(ref _phoneNumber, value);
                ValidateProperty(value);
            }
        }

        #endregion

        #region events and methods
        private List<Sensor> InitializeSensorsAddedToModule(ModuleParameterStatus parameter, List<Sensor> sensorList)
        {
            List<Sensor> retValue = new List<Sensor>();
            if (parameter == ModuleParameterStatus.All)
            {
                retValue = sensorList;
            }
            else // Only applicable in case of TemperatureConductivityModule.
            {
                if (parameter == ModuleParameterStatus.TemperatureOnly)
                {
                    retValue = sensorList.FindAll(t => t.Type == SensorType.Temperature);
                }
                else if (parameter == ModuleParameterStatus.ConductivityOnly)
                {
                    retValue = sensorList.FindAll(t => t.Type == SensorType.Conductivity);
                }
            }
            return retValue;
        }

        public static ValidationResult CheckAcceptableAreaCodes(string phone, ValidationContext context)
        {
            string[] areaCodes = { "760", "442" };
            bool match = false;
            foreach (var ac in areaCodes)
            {
                if (phone != null && phone.Contains(ac))
                { 
                    match = true; 
                    break; 
                }
            }
            if (!match) 
                return new ValidationResult("Only San Diego Area Codes accepted");
            else 
                return ValidationResult.Success;
        }

        #endregion
    }
}
