using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Infrastructure.Commands
{
    public abstract class AwaitableDelegateCommandBase<T> : ICommand
    {
        #region fields and constructors
        private Func<T, Task> _executeMethod;
        private Func<T, bool> _canExecuteMethod;

        public AwaitableDelegateCommandBase(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this._executeMethod = executeMethod;
            this._canExecuteMethod = canExecuteMethod;
        }

        public AwaitableDelegateCommandBase(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this._executeMethod = (args) =>
            {
                executeMethod(args);
                return Task.FromResult<bool>(true);
            };
            this._canExecuteMethod = canExecuteMethod;
        }
        #endregion

        #region ICommand members
        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        async void ICommand.Execute(object parameter)
        {
            await this.Execute((T)parameter);
        }
        #endregion

        #region events and methods
        protected bool CanExecute(T parameter)
        {
            return this._canExecuteMethod == null || this._canExecuteMethod(parameter);
        }

        protected async Task Execute(T parameter)
        {
            await this._executeMethod(parameter);
        }

        public void RaiseCanCommandExecute()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
