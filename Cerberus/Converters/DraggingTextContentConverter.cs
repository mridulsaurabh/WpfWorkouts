using System;
using System.Windows.Data;

namespace Cerberus.Converters
{
    public class DraggingTextContentConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string retValue = string.Empty;
            if (value != null)
            {
                SensorModule temp = value as SensorModule;
                if (temp != null)
                {
                    retValue = string.Format("{0} from {1}", temp.Name, temp.ParentReader);
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
