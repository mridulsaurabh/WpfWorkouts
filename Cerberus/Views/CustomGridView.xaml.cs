using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Infrastructure.Utility;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for CustomGridView.xaml
    /// </summary>
    public partial class CustomGridView : UserControl
    {
        #region fields and constructors
        private object m_TargetItem;
        private ObservableCollection<SensorModule> m_AvailableSensorModules;
        private ListCollectionView m_AvailableSensorModuleCollection;

        public CustomGridView(ObservableCollection<SensorModule> availableSensorModules, bool isToDoSelection = false)
        {
            InitializeComponent();
            customizedSensorsDataGrid.ItemsSource = m_AvailableSensorModules = availableSensorModules;

            m_AvailableSensorModuleCollection = new ListCollectionView(availableSensorModules);
            if (m_AvailableSensorModuleCollection.GroupDescriptions != null)
            {
                m_AvailableSensorModuleCollection.GroupDescriptions.Add(new PropertyGroupDescription("ParentReader"));
            }
            selectionList.ItemsSource = m_AvailableSensorModuleCollection;
            if (isToDoSelection)
            {
                customizedSensorsDataGrid.Visibility = Visibility.Collapsed;
                selectionList.Visibility = Visibility.Visible;
            }
            else
            {
                customizedSensorsDataGrid.Visibility = Visibility.Visible;
                selectionList.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region events and methods
        private void OnWindowSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            List<SensorModule> selectedSensorModules = new List<SensorModule>();
            foreach (SensorModule sModule in selectionList.Items)
            {
                if (sModule.IsMarked)
                {
                    selectedSensorModules.Add(sModule);
                }
            }
            selectionGrid.Visibility = Visibility.Collapsed;

            customizedSensorsDataGrid.ItemsSource = selectedSensorModules;
            customizedSensorsDataGrid.Visibility = Visibility.Visible;
        }

        private void OnWindowCancelButtonClicked(object sender, RoutedEventArgs e)
        {            
            selectionGrid.Visibility = Visibility.Collapsed;

            customizedSensorsDataGrid.ItemsSource = new List<SensorModule>();
            customizedSensorsDataGrid.Visibility = Visibility.Visible;
        }

        private void OnDataGridMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                object selectedItem = customizedSensorsDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    DataGridRow rowBeingDragged = (DataGridRow)customizedSensorsDataGrid.ItemContainerGenerator.ContainerFromItem(selectedItem);
                    if (rowBeingDragged != null)
                    {
                        DragDropEffects finalDropEffect = DragDrop.DoDragDrop(rowBeingDragged, selectedItem, DragDropEffects.Move);
                        if ((finalDropEffect == DragDropEffects.Move) && (m_TargetItem != null))
                        {
                            // A Move drop was accepted
                            SensorModule selectedSensorModule = (SensorModule)selectedItem;
                            SensorModule targetSensorModule = (SensorModule)m_TargetItem;

                            int oldIndex = m_AvailableSensorModules.IndexOf(selectedSensorModule);
                            int newIndex = m_AvailableSensorModules.IndexOf(targetSensorModule);
                            m_AvailableSensorModules.Move(oldIndex, newIndex);

                            m_TargetItem = null;
                        }
                    }
                }
            }
        }

        private void OnDataGridDropTargetChecked(object sender, DragEventArgs e)
        {
            if (UIHelperUtility.FindVisualParent<DataGridRow>(e.OriginalSource as UIElement) == null)
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;

            // Open the dragging PopUp.
            draggingPopup.IsOpen = true;
            Size popupSize = new Size(draggingPopup.ActualWidth, draggingPopup.ActualHeight);
            Point popupOrientation = e.GetPosition(this);
            popupOrientation.Y -= 35;
            draggingPopup.PlacementRectangle = new Rect(popupOrientation, popupSize);
        }

        private void OnDataGridRowDrop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            // Verify that this is a valid drop and then store the drop target
            DataGridRow container = UIHelperUtility.FindVisualParent<DataGridRow>(e.OriginalSource as UIElement) as DataGridRow;
            if (container != null)
            {
                m_TargetItem = container.DataContext;
                e.Effects = DragDropEffects.Move;
                draggingPopup.IsOpen = false;
            }
        }
        #endregion
    }
}
