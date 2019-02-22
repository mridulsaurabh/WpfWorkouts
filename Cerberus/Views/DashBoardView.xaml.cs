using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DashBoardView
    {
        #region fields and constructors
        private DashBoardViewModel _viewModel;
        private NotificationView _notificationView;
        private SensorModuleGridView _sensorModuleGridView;
        private ExperimentGridView _experimentGridView;
        private CustomGridView _customGridView;
        private BusyIndicatorControl _busyIndicator;

        public DashBoardView()
        {
            InitializeComponent();
            this._viewModel = App.CerberusContainer.Resolve<DashBoardViewModel>();
            this.DataContext = _viewModel;
            this.InitializeAndLaunchDashBoardView();
            this._viewModel._eventAggregator.Subscribe<ViewSelectionMessage>(OnCurrentViewSelected);
        }
        #endregion

        #region events and methods

        private void InitializeAndLaunchDashBoardView()
        {
            _busyIndicator = new BusyIndicatorControl();
            if (_busyIndicator != null)
            {
                Grid.SetRow(_busyIndicator, 1);
                Grid.SetRowSpan(_busyIndicator, 3);
                dashBoardGrid.Children.Add(_busyIndicator);
            }
            _busyIndicator.Start("Please wait while application is being initialized !!!!");
        }

        private void OnCurrentViewSelected(ViewSelectionMessage message)
        {
            switch (message.CurrentView)
            {
                case ViewName.DashBoardView:
                    break;
                case ViewName.CustomGridView:
                    _customGridView = new CustomGridView(_viewModel.SensorModules, true);
                    regionNavigationControl.ShowRegion(_customGridView);
                    break;
                case ViewName.ExperimentView:
                    if (_experimentGridView == null)
                    {
                        _experimentGridView = new ExperimentGridView(_viewModel, regionNavigationControl);
                    }
                    regionNavigationControl.ShowRegion(_experimentGridView);
                    break;
                case ViewName.TrendView:
                    if (_sensorModuleGridView == null)
                    {
                        _sensorModuleGridView = new SensorModuleGridView(_viewModel, regionNavigationControl);
                    }
                    regionNavigationControl.ShowRegion(_sensorModuleGridView);
                    break;
                default:
                    break;
            }
        }

        private void OnSelectionViewButtonClicked(object sender, RoutedEventArgs e)
        {
            _customGridView = new CustomGridView(_viewModel.SensorModules, true);
            regionNavigationControl.ShowRegion(_customGridView);
        }

        private void OnArrangeRowsButtonClicked(object sender, RoutedEventArgs e)
        {
            _customGridView = new CustomGridView(_viewModel.SensorModules);
            regionNavigationControl.ShowRegion(_customGridView);
        }

        private void OnShowExperimentButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_experimentGridView == null)
            {
                _experimentGridView = new ExperimentGridView(_viewModel, regionNavigationControl);
            }
            regionNavigationControl.ShowRegion(_experimentGridView);
        }

        private void OnSwitchToDefaultViewButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_sensorModuleGridView == null)
            {
                _sensorModuleGridView = new SensorModuleGridView(_viewModel, regionNavigationControl);
            }
            regionNavigationControl.ShowRegion(_sensorModuleGridView);
        }

        private void OnNotificationIconClicked(object sender, RoutedEventArgs e)
        {
            if (_notificationView == null || !_notificationView.IsOpen)
            {
                _notificationView = new NotificationView(Notifier.Instance);
                _notificationView.Show();
            }
        }

        private void OnCreateExperimentButtonClicked(object sender, RoutedEventArgs e)
        {
            Notifier.Instance.Notify("system", "Experiment Created !!!", Category.Information);

            //STEP 1: CREATE AN EXPERIMENT WITH PASSING ALL THE SENSORS (EXTRACTED FROM SENSOR MODULES) AVAILABLE IN DASHBOARD VIEW
            var availableSensors = new List<Sensor>();
            foreach (var sModule in _viewModel.SensorModules)
            {
                foreach (var sensor in sModule.ModuleSensors)
                {
                    sensor.ID = sModule.Name;
                    sensor.Reader = sModule.ParentReader;
                    availableSensors.Add(sensor);
                }
            }
            ExperimentSetupView experimentView = new ExperimentSetupView(new Experiment(availableSensors));
            regionNavigationControl.ShowRegion(experimentView);
        }

        #endregion

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _sensorModuleGridView = new SensorModuleGridView(_viewModel, regionNavigationControl);
            regionNavigationControl.ShowRegion(_sensorModuleGridView);
            transitionTypeComboBox.ItemsSource = Enum.GetNames(typeof(RegionNavigationControl.RegionTransitionType));
            _busyIndicator.Stop();

            Notifier.Instance.Notify("system", "FIRST Notification arrived !!!", Category.Information);
        }

        private void transitionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            regionNavigationControl.TransitionType = (RegionNavigationControl.RegionTransitionType)Enum.Parse(
                                                        typeof(RegionNavigationControl.RegionTransitionType),
                                                        transitionTypeComboBox.SelectedItem.ToString(),
                                                        true);
        }
    }
}
