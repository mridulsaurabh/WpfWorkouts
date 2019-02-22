using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Data;
using Infrastructure.Commands;
using Infrastructure.Common;

namespace Cerberus
{
    public class Experiment : ModelBase
    {
        #region fields and constructors
        private string m_Name;
        private string m_Description;
        private SensorType m_Type;
        private string m_SelectedUnit;
        private AlgorithmType m_SelectedAlgorithm;
        private List<Sensor> m_SensorsFromAllReaders;
        private List<Sensor> m_SelectedSensors;
        private Alarm m_AlarmValues;
        private LogFile m_Logfile;

        public Experiment(List<Sensor> sensorsFromAllReaders)
        {
            m_Logfile = new LogFile() { Author = "superuser" };
            m_AlarmValues = new Alarm(25, 45);
            m_SelectedSensors = new List<Sensor>();

            m_SensorsFromAllReaders = sensorsFromAllReaders;
            SubscribeToCommands();
        }

        public Experiment(string name, AlgorithmType type, double calculatedValue, string unit, Alarm values, DateTime datetime)
        {
            Name = name;
            SelectedAlgorithm = type;
            CalculatedValue = calculatedValue;
            SelectedUnit = unit;
            AlarmValues = values;
            CreationTime = datetime;

            m_Logfile = new LogFile();
            SubscribeToCommands();
        }

        #endregion

        #region properties and delegates

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                OnPropertyChanged(() => this.Name);
                SetProperty(ref this.m_Name, value);
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                SetProperty(ref this.m_Description, value);
            }
        }

        public SensorType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                SetProperty(ref this.m_Type, value);
                UpdateBindingsOnTypeSelection();
            }
        }

        public DateTime CreationTime
        {
            get;
            set;
        }

        public List<string> AvailableUnits
        {
            get
            {
                return UnitTable.GetAvailableUnits(m_Type);
            }
        }

        public string SelectedUnit
        {
            get
            {
                return m_SelectedUnit;
            }
            set
            {
                SetProperty(ref this.m_SelectedUnit, value);
            }
        }

        public List<Sensor> AvailableSensorsPerType
        {
            get
            {
                var desiredSensors = new List<Sensor>();
                if (m_SensorsFromAllReaders != null)
                {
                    desiredSensors = m_SensorsFromAllReaders.FindMatchAll(o => o.Type == m_Type);
                }
                return desiredSensors;
            }
        }

        public ListCollectionView GroupedSensorsPerType
        {
            get
            {
                var groupedSensors = new ListCollectionView(AvailableSensorsPerType);
                if (groupedSensors.GroupDescriptions != null)
                {
                    groupedSensors.GroupDescriptions.Add(new PropertyGroupDescription("Reader"));
                }
                return groupedSensors;
            }
        }

        public List<Sensor> SelectedSensors
        {
            get
            {
                return m_SelectedSensors;
            }
            set
            {
                SetProperty(ref this.m_SelectedSensors, value);
            }
        }

        public AlgorithmType SelectedAlgorithm
        {
            get
            {
                return m_SelectedAlgorithm;
            }
            set
            {
                SetProperty(ref this.m_SelectedAlgorithm, value);
            }
        }

        public double CalculatedValue
        {
            get;
            set;
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

        public LogFile LogFile
        {
            get
            {
                return m_Logfile;
            }
            set
            {
                SetProperty(ref this.m_Logfile, value);
            }
        }

        public ICommand AddSensorCommand
        {
            get;
            set;
        }

        public ICommand SelectTypeCommand
        {
            get;
            set;
        }

        public ICommand SelectAlgorithmCommand
        {
            get;
            set;
        }

        #endregion

        #region events and methods

        private void SubscribeToCommands()
        {
            AddSensorCommand = new GenericCommand<Sensor>(OnSensorAddedToExperiment);
            SelectTypeCommand = new GenericCommand<string>(OnTypeSelectionChanged);
            SelectAlgorithmCommand = new GenericCommand<string>(OnAlgorithmSelectionChanged);
        }

        private void OnSensorAddedToExperiment(Sensor sensor)
        {
            bool hasAdded = m_SelectedSensors.Any(s => s.ID == sensor.ID);
            if (!hasAdded)
            {
                m_SelectedSensors.Add(sensor);
            }
            else
            {
                m_SelectedSensors.Remove(sensor);
            }
            OnPropertyChanged("SelectedSensors");
        }

        private void OnTypeSelectionChanged(string currentType)
        {
            if (!String.IsNullOrEmpty(currentType))
            {
                var type = (SensorType)Enum.Parse(typeof(SensorType), currentType);
                Type = type;
            }
        }

        private void OnAlgorithmSelectionChanged(string appliedAlgorithm)
        {
            if (!String.IsNullOrEmpty(appliedAlgorithm))
            {
                var currentAlgorithm = (AlgorithmType)Enum.Parse(typeof(AlgorithmType), appliedAlgorithm);
                SelectedAlgorithm = currentAlgorithm;
            }
        }

        private void UpdateBindingsOnTypeSelection()
        {
            OnPropertyChanged("AvailableUnits");
            OnPropertyChanged("GroupedSensorsPerType");
            SelectedAlgorithm = AlgorithmType.None;
            SelectedSensors = new List<Sensor>();
        }

        #endregion
    }
}
