using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Infrastructure.Common
{
    public class NotificationObject : INotifyPropertyChanged, INotifyDataErrorInfo
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            MemberExpression expression = propertyExpression.Body as MemberExpression;
            if (expression != null)
            {
                this.OnPropertyChanged(expression.Member.Name);
            }
        }

        #endregion

        #region INotifyDataErrorInfo Members
        // this interface gives more flexibility on model validation i.e when to validate properties 
        // for asynchronous validation we could add a ValidateAsync and ValidatePropertyAsync methods
        // Use a ConcurrentDictionary instead of regular ones as our error cache.

        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private object _lock = new object();
        
        public IEnumerable GetErrors(string propertyName)
        {
            // Its called by binding engine to retrieve errors.
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (_errors.ContainsKey(propertyName) && (_errors[propertyName] != null) && _errors[propertyName].Count > 0)
                    return _errors[propertyName].ToList();
                else
                    return null;
            }
            else
            {
                // if property name is null then return all errors available for the model associated to various properties.
                return _errors.SelectMany(err => err.Value.ToList());
            }
        }

        public bool HasErrors
        {
            // set to true incase of any errors in model at the moment. Nevertheless this property
            // is not used by the binding engine so we can utilize it for our own use.
            get
            {
                return _errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
            }
        }

        protected void OnErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }       

        protected void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                validationContext.MemberName = propertyName;
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateProperty(value, validationContext, validationResults);

                //clear previous _errors from tested pr operty  
                if (_errors.ContainsKey(propertyName))
                {
                    _errors.Remove(propertyName);
                }
                OnErrorsChanged(propertyName);
                HandleValidationResults(validationResults);
            }
        }
        
        protected void Validate()
        {
            // API to validate the whole model including all properties inherited by this.
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                //clear all previous _errors  
                var propNames = _errors.Keys.ToList();
                _errors.Clear();
                propNames.ForEach(pn => OnErrorsChanged(pn));
                HandleValidationResults(validationResults);
            }
        }

        private void HandleValidationResults(List<ValidationResult> validationResults)
        {
            //Group validation results by property names  
            var resultsByPropNames = from res in validationResults
                                     from mname in res.MemberNames
                                     group res by mname into g
                                     select g;
            //add _errors to dictionary and inform binding engine about _errors  
            foreach (var prop in resultsByPropNames)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                _errors.Add(prop.Key, messages);
                OnErrorsChanged(prop.Key);
            }
        }

        #endregion
    }
}
