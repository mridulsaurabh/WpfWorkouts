using System;
using System.Windows.Data;
using System.Windows;

namespace Cerberus.Converters
{
    public class NotificationToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility retValue = Visibility.Hidden;
            if (value != null && value is Notification)
            {
                retValue = (String.IsNullOrEmpty((value as Notification).Source)) ? Visibility.Collapsed : Visibility.Visible;
            }
            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
