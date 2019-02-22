using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Infrastructure.Commands
{
    public class AwaitableDelegateCommand : AwaitableDelegateCommandBase<object>
    {
        public AwaitableDelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {

        }

        public AwaitableDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {

        }

        public bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public async Task Execute(object parameter)
        {
            await base.Execute(parameter);
        }
    }

    public class AwaitableDelegateCommand<T> : AwaitableDelegateCommandBase<T>
    {
        public AwaitableDelegateCommand(Action<T> executeMethod)
            : base(executeMethod, _ => true)
        {

        }

        public AwaitableDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {

        }
        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public async Task Execute(T parameter)
        {
            await base.Execute(parameter);
        }
    }
}
