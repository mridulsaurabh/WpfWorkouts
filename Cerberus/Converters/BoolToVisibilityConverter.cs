using System;
using System.Windows.Data;
using System.Windows;

namespace Cerberus.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility retValue = Visibility.Hidden;
            if (value != null && value is bool)
            {
                retValue = ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
