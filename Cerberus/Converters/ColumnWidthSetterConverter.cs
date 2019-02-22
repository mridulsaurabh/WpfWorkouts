using System;
using System.Windows.Data;
using System.Windows;

namespace Cerberus.Converters
{
    public class ColumnWidthSetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GridLength currentWidth = new GridLength();
            if (value != null && value is bool)
            {
                bool hasExpanded = (bool)value;
                currentWidth = (hasExpanded) ? new GridLength(0) : new GridLength(0.5, GridUnitType.Star);
            }
            return currentWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
