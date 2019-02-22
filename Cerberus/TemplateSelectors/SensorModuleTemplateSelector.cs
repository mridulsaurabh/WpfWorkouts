using System.Windows.Controls;
using System.Windows;

namespace Cerberus
{
    public class SensorModuleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PressureModuleTemplate
        {
            get;
            set;
        }

        public DataTemplate TemperatureConductivityModuleTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var module = item as SensorModule;
            if (module != null)
            {
                switch (module.Type)
                {
                    case ModuleType.PressureModule:
                        {
                            return PressureModuleTemplate;
                        }
                    case ModuleType.TemperatureConductivityModule:
                        {
                            return TemperatureConductivityModuleTemplate;
                        }
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
