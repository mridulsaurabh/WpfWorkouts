using System;

namespace Cerberus
{
    public class NotificationReceivedEventArgs : EventArgs
    {
        public NotificationReceivedEventArgs(Notification notification)
        {
            CurrentNotification = notification;
        }

        public Notification CurrentNotification
        {
            get;
            private set;
        }
    }
}
