using Infrastructure.Utility;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for MainLayoutControl.xaml
    /// </summary>
    public partial class SensorModuleGridView : UserControl
    {
        #region fields and constructors
        private const string SUPERVISORYPASSWORD = "123";
        private Button m_EditButton;
        private TrendView m_TrendView;
        private RegionNavigationControl m_NavigationControl;
        private DashBoardViewModel m_ViewModel;

        public SensorModuleGridView(DashBoardViewModel viewModel, RegionNavigationControl navigationControl)
        {
            InitializeComponent();
            m_ViewModel = viewModel;
            DataContext = m_ViewModel;
            m_NavigationControl = navigationControl;
        }
        #endregion

        #region events and methods

        private void OnEditButtonClicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                m_EditButton = btn;
                var depObject = (DependencyObject)e.OriginalSource;
                while ((depObject != null) && !(depObject is DataGridRow))
                {
                    depObject = VisualTreeHelper.GetParent(depObject);
                }

                if (depObject != null && depObject is DataGridRow)
                {
                    var currentRow = (DataGridRow)depObject;
                    currentRow.DetailsVisibility = Visibility.Visible;
                }
                m_EditButton.IsEnabled = false;
            }
        }

        private void OnSaveAndStartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_ViewModel != null && !m_ViewModel.HasPassword)
            {
                var authenticationWindow = new UserCredentialWindow(m_ViewModel);
                authenticationWindow.okayButton.Click += (oo, es) =>
                {
                    if (authenticationWindow.maskedTextBox.Password.Equals(SUPERVISORYPASSWORD))
                    {
                        authenticationWindow.warningTextBlock.Visibility = Visibility.Collapsed;
                        authenticationWindow.Close();
                        OnSensorSettingsSaved(e, true);
                    }
                    else
                    {
                        authenticationWindow.warningTextBlock.Visibility = Visibility.Visible;
                    }
                };
                authenticationWindow.ShowDialog();
            }
            else
            {
                OnSensorSettingsSaved(e, true);
            }
        }

        private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_ViewModel != null && !m_ViewModel.HasPassword)
            {
                var authenticationWindow = new UserCredentialWindow(m_ViewModel);
                authenticationWindow.okayButton.Click += (oo, es) =>
                {
                    if (authenticationWindow.maskedTextBox.Password.Equals(SUPERVISORYPASSWORD))
                    {
                        authenticationWindow.warningTextBlock.Visibility = Visibility.Collapsed;
                        authenticationWindow.Close();
                        OnSensorSettingsSaved(e);
                    }
                    else
                    {
                        authenticationWindow.warningTextBlock.Visibility = Visibility.Visible;
                    }
                };
                authenticationWindow.ShowDialog();
            }
            else
            {
                OnSensorSettingsSaved(e);
            }
        }

        private void OnSensorSettingsSaved(RoutedEventArgs e, bool saveAndStart = false)
        {
            //STEP 1: 
            //UPDATE THE CELLS WITH ENTERED VALUES FOR THE SELECTED ROW

            //STEP 2:
            //IF SAVING IS SUCCESSFUL THEN HIDE THE ROW DETAILS TEMPLATE FOR THE SELECTED ROW AND UPDATE THE SETTING PANEL TEMPLATE ALLOWING ONLY TO CHANGE LOG SETTINGS.        

            bool hasValidationErrors = this.m_ViewModel.SensorModules.Any(o => o.HasErrors);

            var dep = (DependencyObject)e.OriginalSource;
            // iteratively traverse the visual tree upwards looking for the clicked row.
            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            // if we found the clicked row
            if (dep != null && dep is DataGridRow)
            {
                var currentRow = (DataGridRow)dep;
                currentRow.DetailsVisibility = (hasValidationErrors) ? Visibility.Visible : Visibility.Collapsed;

                if (currentRow.Item is SensorModule)
                {
                    var selectedSensorModule = m_ViewModel.SensorModules.First(t => t.Name == (currentRow.Item as SensorModule).Name);
                    if (selectedSensorModule != null && saveAndStart)
                    {
                        selectedSensorModule.IsActivated = (hasValidationErrors) ? false : true;
                    }
                }
            }
            m_EditButton.IsEnabled = true;
        }

        private void OnStopButtonClicked(object sender, RoutedEventArgs e)
        {
            var dep = (DependencyObject)e.OriginalSource;
            // iteratively traverse the visual tree upwards looking for the clicked row.
            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep != null && m_ViewModel != null && dep is DataGridRow)
            {
                var currentRow = (DataGridRow)dep;
                currentRow.DetailsVisibility = Visibility.Collapsed;

                if (currentRow.Item is SensorModule)
                {
                    var selectedSensorModule = m_ViewModel.SensorModules.First(t => t.Name == (currentRow.Item as SensorModule).Name);
                    if (selectedSensorModule != null)
                    {
                        selectedSensorModule.IsActivated = false;
                    }
                }
                else if (currentRow.Item is Experiment)
                {
                    var selectedExpt = m_ViewModel.AvailableExperiments.FindMatch(t => t.Name == (currentRow.Item as Experiment).Name);
                    if (selectedExpt != null)
                    {
                        m_ViewModel.AvailableExperiments.Remove(selectedExpt);
                    }
                }
            }
            m_EditButton.IsEnabled = true;
        }

        private void OnCancelChangesButtonClicked(object sender, RoutedEventArgs e)
        {
            var dep = (DependencyObject)e.OriginalSource;
            // iteratively traverse the visual tree upwards looking for the clicked row.
            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            // if we found the clicked row
            if (dep != null && dep is DataGridRow)
            {
                var row = (DataGridRow)dep;
                row.DetailsVisibility = Visibility.Collapsed;
            }
            m_EditButton.IsEnabled = true;
        }

        private void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            var sModule = ((Hyperlink)e.OriginalSource).CommandParameter as SensorModule;
            if (sModule != null)
            {
                var desiredModule = m_ViewModel.SensorModules.FindMatch(t => t.Name == sModule.Name);
                Notifier.Instance.Notify(desiredModule.Name, string.Format("Unable to recieve the data for the sensor :{0}", desiredModule.Name), Category.Warning);
            }
            if (m_TrendView == null)
            {
                m_TrendView = new TrendView(App.CerberusContainer.Resolve<TrendViewViewModel>());
            }
            m_NavigationControl.ShowRegion(m_TrendView);        

        }

        #endregion
    }
}
