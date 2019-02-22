using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace Cerberus.Converters
{
    public class SensorModulesPerTypeCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<string> availableSensorNames = new List<string>();
            if (value != null && value is List<Sensor>)
            {
                var availableSensnors = value as List<Sensor>;
                foreach (var s in availableSensnors)
                {
                    availableSensorNames.Add(s.ID);
                }
            }
            return availableSensorNames;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
