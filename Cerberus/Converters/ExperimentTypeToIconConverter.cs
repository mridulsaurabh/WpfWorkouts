using Infrastructure;
using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Cerberus.Converters
{
    public class ExperimentTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage retValue = new BitmapImage();
            if (value != null && value is AlgorithmType)
            {
                var currentType = (AlgorithmType)value;
                switch (currentType)
                {
                    case AlgorithmType.DifferentialPressure:
                        retValue = new BitmapImage(new Uri("/Images/DP.PNG", UriKind.RelativeOrAbsolute));
                        break;
                    case AlgorithmType.DifferentialTemperature:
                        retValue = new BitmapImage(new Uri("/Images/DT.PNG", UriKind.RelativeOrAbsolute));
                        break;
                    case AlgorithmType.TransmembranePressure:
                        retValue = new BitmapImage(new Uri("/Images/TMP.PNG", UriKind.RelativeOrAbsolute));
                        break;
                    case AlgorithmType.CustomEquation:
                        retValue = new BitmapImage(new Uri("/Images/CE.PNG", UriKind.RelativeOrAbsolute));
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
