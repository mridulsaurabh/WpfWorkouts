using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Common
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            else
            {
                storage = value;
                this.OnPropertyChanged(propertyName);
                return true;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            MemberExpression expression = propertyExpression.Body as MemberExpression;
            if(expression != null)
            {
                this.OnPropertyChanged(expression.Member.Name);
            }
        }

        #endregion

        #region INotifyDataErrorInfo Members
        //private ConcurrentDictionary<string, List<string>> errors =
        //                                                   new ConcurrentDictionary<string, List<string>>();

        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        //public void OnErrorChanged(string propertyName)
        //{
        //    if (this.ErrorsChanged != null)
        //    {
        //        this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        //    }
        //}

        //public bool HasErrors
        //{
        //    get
        //    {
        //        return errors.Any(p => p.Value != null && p.Value.Count > 0);
        //    }
        //}

        //protected IEnumerable GetErrors(string propertyName)
        //{
        //    if (errors.ContainsKey(propertyName))
        //    {
        //        return errors[propertyName];
        //    }
        //    return null;
        //}

        //protected void AddError(string propertyName, string error)
        //{
        //    if (!errors.ContainsKey(propertyName))
        //    {
        //        errors[propertyName] = new List<string>();
        //    }
        //    if (!errors[propertyName].Contains(error))
        //    {
        //        errors[propertyName].Add(error);
        //        if (ErrorsChanged != null)
        //        {
        //            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        //        }
        //    }
        //}
        #endregion
    }
}
