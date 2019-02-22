using System;
using System.Collections.ObjectModel;
using Infrastructure.Common;

namespace Cerberus
{
    public class Notifier : ModelBase
    {
        #region fields and constructors
        private int m_MaxQueueCount = 10;
        private ObservableCollection<Notification> m_Notifications;
        private static Notifier m_SoleNotifier = null;

        private Notifier()
        {
            m_SoleNotifier = this;
            m_Notifications = new ObservableCollection<Notification>();
        }

        #endregion

        #region properties and delegates

        public static Notifier Instance
        {
            get
            {
                if (m_SoleNotifier == null)
                {
                    new Notifier();
                }
                return m_SoleNotifier;
            }
        }

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return m_Notifications;
            }
            private set
            {
                SetProperty(ref this.m_Notifications, value);
            }
        }

        public event EventHandler<NotificationReceivedEventArgs> NotificationReceived;

        public event Action<object, NotificationReceivedEventArgs> NotificationAcknowledged;

        #endregion

        #region events and methods

        public void Notify(string source, string message, Category category)
        {
            Notification notification = new Notification(source, message, category, DateTime.Now);
            m_Notifications.Insert(0, notification);
            OnNotificationReceived(this, new NotificationReceivedEventArgs(notification));
            OnPropertyChanged("Notifications");
        }

        public void RemoveNotification(Notification aNotification)
        {
            if (m_Notifications != null && m_Notifications.Count > 0)
            {
                var removableNotification = m_Notifications.FindMatch
                                (t => t.Category == aNotification.Category && t.DateAndTime == aNotification.DateAndTime);
                if (removableNotification != null)
                {
                    m_Notifications.Remove(removableNotification);
                    OnNotificationAcknowledged(this, new NotificationReceivedEventArgs(removableNotification));
                    if (m_Notifications.Count == 0)
                    {
                        OnNotificationAcknowledged(this, new NotificationReceivedEventArgs(null));
                    }
                    OnPropertyChanged("Notifications");
                }
            }
        }

        private void OnNotificationReceived(object sender, NotificationReceivedEventArgs e)
        {
            EventHandler<NotificationReceivedEventArgs> handler = NotificationReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnNotificationAcknowledged(object sender, NotificationReceivedEventArgs e)
        {
            Action<object, NotificationReceivedEventArgs> handler = NotificationAcknowledged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
