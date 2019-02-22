using Infrastructure;
using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace Cerberus.Converters
{
    public class CellStyleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage retValue = new BitmapImage();
            if (value != null && value is SensorType)
            {
                var currentType = (SensorType)value;
                switch (currentType)
                {
                    case SensorType.Pressure:
                        retValue = new BitmapImage(new Uri("/Images/PressureIcon.bmp", UriKind.RelativeOrAbsolute));
                        break;
                    case SensorType.Temperature:
                        retValue = new BitmapImage(new Uri("/Images/TemperatureIcon.bmp", UriKind.RelativeOrAbsolute));
                        break;
                    case SensorType.Conductivity:
                        retValue = new BitmapImage(new Uri("/Images/ConductivityIcon.bmp", UriKind.RelativeOrAbsolute));
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
