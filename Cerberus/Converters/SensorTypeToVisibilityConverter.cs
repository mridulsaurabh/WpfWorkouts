using System;
using System.Windows.Data;
using System.Windows;
using Infrastructure;

namespace Cerberus.Converters
{
    public class SensorTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility retValue = Visibility.Hidden;
            if(value != null && !String.IsNullOrEmpty((string)parameter))
            {
                SensorType type = (SensorType)value;
                string param = (string)parameter;
                switch (param)
                {
                    case "DP":
                        retValue = (type == SensorType.Pressure) ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "TMP":
                        retValue = (type == SensorType.Pressure) ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "DT":
                        retValue = (type == SensorType.Temperature) ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "CE":
                        retValue = (type != SensorType.None) ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    default:                        
                        break;
                }
            }
            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
