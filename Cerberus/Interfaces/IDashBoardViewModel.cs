using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Cerberus.Interfaces
{
    interface IDashBoardViewModel
    {
        ObservableCollection<SensorModule> SensorModules
        {
            get;
            set;
        }

        bool HasPassword
        {
            get;
            set;
        }

        ICommand SaveCommand
        {
            get;
            set;
        }

        void UpdateSourceContext(object superItem);
    }
}
