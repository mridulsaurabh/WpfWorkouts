using System;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Infrastructure.Commands
{
    /// <summary>
    /// eneral purpose templated command class, for use by command buttons
    /// </summary>
    public class GenericCommand : ICommand
    {
        #region field and constructors
        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecutePredicate;
        public event EventHandler CanExecuteChanged;
        public GenericCommand(Action<object> execute)
            : this(execute, null)
        {

        }
        public GenericCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null && canExecute == null)
            {
                throw new ArgumentNullException("GenericCommand has null arguments");
            }
            _executeAction = execute;
            _canExecutePredicate = canExecute;
        }

        #endregion
        #region ICommand members
        public bool CanExecute(object parameter)
        {
            return _canExecutePredicate(parameter);
        }
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Cannot execute now");
            }
            _executeAction(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }


    /// <summary>
    /// General purpose generic templated command class, for use by command buttons
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCommand<T> : ICommand
    {
        #region field and constructors
        private readonly Action<T> m_ExecuteMethod = null;
        private readonly Func<T, bool> m_CanExecuteMethod = null;
        private EventHandler m_CanExecuteChanged = null;

        /// <summary>
        /// Constructor that registers ExecuteMethod
        /// </summary>
        /// <param name="executeMethod"></param>
        public GenericCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        /// <summary>
        /// Constructor that registers ExecuteMethod and CanExecuteMethod
        /// </summary>
        /// <param name="executeMethod"></param>
        /// <param name="canExecuteMethod"></param>
        public GenericCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null && canExecuteMethod == null)
            {
                throw new ArgumentNullException("GenericCommand has null arguments");
            }
            m_ExecuteMethod = executeMethod;
            m_CanExecuteMethod = canExecuteMethod;
        }
        #endregion
        #region ICommand members
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return (m_CanExecuteMethod == null) ? true : m_CanExecuteMethod((T)parameter);
        }
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        void ICommand.Execute(object parameter)
        {
            if (m_ExecuteMethod != null)
            {
                try
                {
                    m_ExecuteMethod((T)parameter);
                    FireCanExecuteChanged();
                }
                catch (Exception ex)
                {
                    string methodName = m_ExecuteMethod.Method != null ? m_ExecuteMethod.Method.Name : "Unknown";
                    Console.WriteLine("Exception occurred at method " + methodName);
                }
            }

        }
        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                m_CanExecuteChanged += value;
            }
            remove
            {
                m_CanExecuteChanged -= value;
            }
        }
        /// <summary>
        /// Notifies registered listners about command can execute
        /// </summary>
        public void FireCanExecuteChanged()
        {
            if (m_CanExecuteChanged != null)
            {
                m_CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
