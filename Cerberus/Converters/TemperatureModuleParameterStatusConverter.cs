using Infrastructure;
using System;
using System.Windows.Data;

namespace Cerberus.Converters
{
    public class TemperatureModuleParameterStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool retValue = false;
            if (value != null && value is ModuleParameterStatus)
            {
                var tempValue = (ModuleParameterStatus)value;
                retValue = (tempValue == ModuleParameterStatus.ConductivityOnly) ? false : true;
            }
            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ModuleParameterStatus retValue = ModuleParameterStatus.All;
            if (value != null && !(bool)value)
            {
                retValue = ModuleParameterStatus.ConductivityOnly;
            }
            return retValue;
        }
    }
}
