using System;
using System.Windows.Data;
using System.Windows;
using Infrastructure;

namespace Cerberus.Converters
{
    public class TemperatureCompensationBoxVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility retValue = Visibility.Hidden;
            if (value != null && value is SensorType)
            {
                retValue = ((SensorType)value == SensorType.Conductivity) ? Visibility.Visible : Visibility.Collapsed;
            }
            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
