using System.Collections.Generic;
using System.ComponentModel;
using Infrastructure.Utility;

namespace Cerberus
{
    public static class UnitTable
    {
        public enum Temperature
        {
            [Description("°C")]
            Celcius,
            [Description("°F")]
            Fahrenheit,
            [Description("K")]
            Kelvin
        }

        public enum Pressure
        {
            [Description("psig")]
            PoundsPerSquareInch,
            [Description("KPa")]
            Pascal,
            [Description("bar")]
            Bar,
            [Description("mm Hg")]
            MillimeterOfMercury
        }

        public enum Conductivity
        {
            [Description("µS")]
            MicroSiemens,
            [Description("mS")]
            MilliSiemens,
            [Description("PPM KCl")]
            PartsPerMillionOfKCl,
            [Description("PPM NaCl")]
            PartsPerMillionOfNaCl
        }

        public static List<string> GetAvailableUnits(SensorType type)
        {
            List<string> availableUnits = new List<string>();
            if (type != null)
            {
                switch (type)
                {
                    case SensorType.Pressure:
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Pressure.PoundsPerSquareInch));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Pressure.Pascal));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Pressure.Bar));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Pressure.MillimeterOfMercury));
                        break;
                    case SensorType.Temperature:
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Temperature.Celcius));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Temperature.Fahrenheit));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Temperature.Kelvin));
                        break;
                    case SensorType.Conductivity:
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Conductivity.MicroSiemens));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Conductivity.MilliSiemens));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Conductivity.PartsPerMillionOfKCl));
                        availableUnits.Add(UIHelperUtility.GetEnumDescription(Conductivity.PartsPerMillionOfNaCl));
                        break;
                    default:
                        break;
                }
            }
            return availableUnits;
        }
    }
}
