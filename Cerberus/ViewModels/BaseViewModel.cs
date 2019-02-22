using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Infrastructure.Utility;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Cerberus
{
    public abstract class BaseViewModel : ViewModelBase
    {
        #region fields and constructors
        private DateTime m_CurrentTime;
        private DispatcherTimer m_DTimer;
        private Notification m_LatestNotification = new Notification();
        private int m_TotalNotifications;
        private ObservableCollection<SensorModule> m_SensorModules = new ObservableCollection<SensorModule>();
        private ObservableCollection<Experiment> m_AvailableExperiments = new ObservableCollection<Experiment>();
        private IWorkAsync _asyncService;
        private IEventAggregator _eventAggregator;

        public BaseViewModel(IWorkAsync service, IEventAggregator eventAggregator)
            : base(service, eventAggregator)
        {
            m_DTimer = new DispatcherTimer();
            m_DTimer.Interval = new TimeSpan(0, 0, 1);
            m_DTimer.Tick += new EventHandler(OnTimerTick);
            m_DTimer.Start();
            this._asyncService = service;
            this._eventAggregator = eventAggregator;
            InitializeAttachedSensorsAndExpeiments();
            SubscribeToEvents();
        }
        #endregion

        #region properties and delegates

        private string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set
            {
                SetProperty(ref _busyMessage, value);
            }
        }


        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetProperty(ref _isBusy, value);
            }
        }

        public ObservableCollection<SensorModule> SensorModules
        {
            get
            {
                return m_SensorModules;
            }
            set
            {
                SetProperty(ref m_SensorModules, value);
            }
        }

        public ObservableCollection<Experiment> AvailableExperiments
        {
            get
            {
                return m_AvailableExperiments;
            }
            set
            {
                SetProperty(ref m_AvailableExperiments, value);
            }
        }

        public Notification LatestNotification
        {
            get
            {
                return m_LatestNotification;
            }
            set
            {
                SetProperty(ref m_LatestNotification, value);
            }
        }

        public int TotalNotifications
        {
            get
            {
                return m_TotalNotifications;
            }
            set
            {
                SetProperty(ref m_TotalNotifications, value);
            }
        }

        public DateTime CurrentSystemTime
        {
            get
            {
                return m_CurrentTime;
            }
            set
            {
                SetProperty(ref m_CurrentTime, value);
            }
        }

        #endregion

        #region events and methods

        private void InitializeAttachedSensorsAndExpeiments()
        {
            if (this._asyncService != null)
            {
                this._asyncService.RunAsync(
                    () =>
                    {
                        m_SensorModules.Add(new SensorModule(ModuleType.TemperatureConductivityModule, "Reader1", AddSensors(ModuleType.TemperatureConductivityModule))
                        {
                            Name = "Name01",
                            DateAndTime = DateTime.Now
                        });
                        
                        m_SensorModules.Add(new SensorModule(ModuleType.PressureModule, "Reader1", AddSensors(ModuleType.PressureModule))
                        {
                            Name = "PName0",
                            DateAndTime = DateTime.Now
                        });

                        m_SensorModules.Add(new SensorModule(ModuleType.TemperatureConductivityModule, "Reader1", AddSensors(ModuleType.TemperatureConductivityModule))
                        {
                            Name = "Name03",
                            DateAndTime = DateTime.Now,
                            CurrentModuleParameter = ModuleParameterStatus.ConductivityOnly
                        });

                        for (int i = 0; i < 3; i++)
                        {
                            m_SensorModules.Add(new SensorModule(ModuleType.PressureModule, "Reader2", AddSensors(ModuleType.PressureModule))
                            {
                                Name = "PSensor" + i.ToString(),
                                DateAndTime = DateTime.Now
                            });
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            m_SensorModules.Add(new SensorModule(ModuleType.TemperatureConductivityModule, "Reader3", AddSensors(ModuleType.TemperatureConductivityModule))
                            {
                                Name = "T/CName0" + i.ToString(),
                                DateAndTime = DateTime.Now,
                                CurrentModuleParameter = ModuleParameterStatus.ConductivityOnly
                            });
                        }

                        m_AvailableExperiments.Add(new Experiment("exptOne", AlgorithmType.DifferentialPressure, 196, UIHelperUtility.GetEnumDescription(UnitTable.Pressure.PoundsPerSquareInch), new Alarm(13, 26), DateTime.Now));
                        m_AvailableExperiments.Add(new Experiment("exptTwo", AlgorithmType.DifferentialTemperature, 68, UIHelperUtility.GetEnumDescription(UnitTable.Temperature.Celcius), new Alarm(5, 12), DateTime.Now));
                        m_AvailableExperiments.Add(new Experiment("exptThree", AlgorithmType.TransmembranePressure, 36, UIHelperUtility.GetEnumDescription(UnitTable.Pressure.Bar), new Alarm(1, 20), DateTime.Now));
                        m_AvailableExperiments.Add(new Experiment("expt4", AlgorithmType.DifferentialPressure, 123, UIHelperUtility.GetEnumDescription(UnitTable.Pressure.MillimeterOfMercury), new Alarm(5, 8), DateTime.Now));
                        m_AvailableExperiments.Add(new Experiment("expt5", AlgorithmType.CustomEquation, 8, UIHelperUtility.GetEnumDescription(UnitTable.Conductivity.PartsPerMillionOfNaCl), new Alarm(11, 22), DateTime.Now));

                    },
                    () =>
                    { 
                        Notifier.Instance.Notify("Application", "Sensors loaded.", Category.Information);
                    },
                    handleError: (o) => { throw o.InnerException; });
            }
        }

        private void SubscribeToEvents()
        {
            Notifier.Instance.NotificationReceived += new EventHandler<NotificationReceivedEventArgs>(OnNewNotificationReceived);
            Notifier.Instance.NotificationAcknowledged += OnNotificationAcknowledged;
        }

        private List<Sensor> AddSensors(ModuleType mType)
        {
            var retValue = new List<Sensor>();
            Random rand = new Random();
            if (mType == ModuleType.PressureModule)
            {
                retValue.Add(new Sensor(SensorType.Pressure)
                {
                    PresentValue = rand.Next(100, 250),
                    Unit = UIHelperUtility.GetEnumDescription(UnitTable.Pressure.PoundsPerSquareInch),
                    AlarmValues = new Alarm(rand.Next(0, 17), rand.Next(15, 20))
                });
            }
            else if (mType == ModuleType.TemperatureConductivityModule)
            {
                // ADD TWO SENSORS OF TYPE TEMPERATURE AND CONDUCTIVITY
                retValue.Add(new Sensor(SensorType.Conductivity)
                {
                    PresentValue = rand.Next(100, 250),
                    Unit = UIHelperUtility.GetEnumDescription(UnitTable.Conductivity.PartsPerMillionOfNaCl),
                    AlarmValues = new Alarm(rand.Next(0, 15), rand.Next(5, 35))
                });

                retValue.Add(new Sensor(SensorType.Temperature)
                {
                    PresentValue = rand.Next(100, 250),
                    Unit = UIHelperUtility.GetEnumDescription(UnitTable.Temperature.Fahrenheit),
                    AlarmValues = new Alarm(rand.Next(0, 21), rand.Next(15, 25))
                });
            }
            return retValue;
        }

        private void OnNewNotificationReceived(object sender, NotificationReceivedEventArgs e)
        {
            if (e != null && e.CurrentNotification != null)
            {
                ++TotalNotifications;
                LatestNotification = e.CurrentNotification;
                var desiredSModule = m_SensorModules.FindMatch(t => t.Name == e.CurrentNotification.Source);
                if (desiredSModule != null)
                {
                    desiredSModule.HasNotifications = true;
                }
            }
        }

        private void OnNotificationAcknowledged(object sender, NotificationReceivedEventArgs e)
        {
            if (e.CurrentNotification != null)
            {
                // CLEAR THE FLAG "HasNotifciations" FROM THE RESPECTIVE SENSOR MODULE AS SOON NOTIFICATION IS ACKNOWLEDGED BY THE USER.
                var desiredSModule = m_SensorModules.FindMatch(t => t.Name == e.CurrentNotification.Source);
                if (desiredSModule != null)
                {
                    desiredSModule.HasNotifications = false;
                }

                // ASSIGN NEXT AVAILABLE NOTIFICATION FROM NOTIFIER COLLECTION TO THE LATEST NOTIFICATION FOR FLASHING CONTROL.
                // IF THERE ARE NO NOTIFICATIONS LEFT IN THE COLLECTION THEN ASSIGN A NULL NOTIFICATION WHICH WILL INDICATE FLASHING CONTROL TO GO OFF.
                var availableNotifications = Notifier.Instance.Notifications;
                if (availableNotifications != null && availableNotifications.Count > 0)
                {
                    var nextNotificationInTheList = availableNotifications.ElementAt(0);
                    LatestNotification = nextNotificationInTheList;
                }
                --TotalNotifications;
            }
            else
            {
                LatestNotification = new Notification();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            CurrentSystemTime = DateTime.Now;
        }

        #endregion
    }
}
