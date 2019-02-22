using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cerberus
{
    public partial class NotificationView : Window
    {
        #region fields and constructors
        private Notifier m_Notifier; 
        public NotificationView(Notifier notifier)
        {
            InitializeComponent();
            m_Notifier = notifier;
            DataContext = m_Notifier;            
        }
        #endregion

        #region properties and delegates
        public bool IsOpen
        {
            get;
            private set;
        }
        #endregion

        #region events and methods
        private void OnBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            notificationViewDataGrid.SelectedItem = null;
        }

        private void OnNotificationWindowLoaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
        }

        private void OnNotificationWindowUnLoaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }       

        private void OnAcknowledgeButtonClicked(object sender, RoutedEventArgs e)
        {
            var depObject = (DependencyObject)e.OriginalSource;
            while ((depObject != null) && !(depObject is DataGridRow))
            {
                depObject = VisualTreeHelper.GetParent(depObject);
            }

            if (depObject != null && depObject is DataGridRow)
            {
                var currentRow = (DataGridRow)depObject;
                currentRow.DetailsVisibility = Visibility.Collapsed;

                // REMOVE THE NOTIFICATION FROM THE NOTIFIER COLLECTION AS IT HAS BEEN ACKNOWLEDGED BY THE USER.               
                var removableNotification = currentRow.Item as Notification;
                m_Notifier.RemoveNotification(removableNotification);
            }
        }
        #endregion
    }
}
