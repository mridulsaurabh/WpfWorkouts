using System.Windows.Controls;
using System.Windows;
using Infrastructure;

namespace Cerberus
{
    public class AlgorithmTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate
        {
            get;
            set;
        }

        public DataTemplate DifferentialPressureTemplate
        {
            get;
            set;
        }

        public DataTemplate TransmembranePressureTemplate
        {
            get;
            set;
        }

        public DataTemplate DifferentialTemperatureTemplate
        {
            get;
            set;
        }

        public DataTemplate CustomEquationTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var algoType = (AlgorithmType)item;
            if (algoType != null)
            {
                switch (algoType)
                {
                    case AlgorithmType.DifferentialPressure:
                        return DifferentialPressureTemplate;                      
                    case AlgorithmType.TransmembranePressure:
                        return TransmembranePressureTemplate;                      
                    case AlgorithmType.DifferentialTemperature:
                        return DifferentialTemperatureTemplate;                    
                    case AlgorithmType.CustomEquation:
                        return CustomEquationTemplate;                       
                    default:
                        return DefaultTemplate;
                        
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
