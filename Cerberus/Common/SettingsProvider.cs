using Cerberus.Interfaces;
using Cerberus.Properties;
using System.Collections.Generic;
using System.Reflection;

namespace Cerberus.Common
{
    public class SettingsProvider : ISettingsProvider
    {
        public object GetSetting(string key)
        {
            return Settings.Default[key];
        }

        public T GetSetting<T>(string key)
        {
            return (T)Settings.Default[key];
        }

        public void SetSetting<T>(string key, T value)
        {
            Settings.Default[key] = value;
        }

        public void Save()
        {
            Settings.Default.Save();
        }

        public void Reset()
        {
            Settings.Default.Reset();
        }

        public IEnumerable<PropertyInfo> GetAllProperties()
        {
            return Settings.Default.GetType().GetProperties();
        }

        public PropertyInfo GetProperty(string name)
        {
            return Settings.Default.GetType().GetProperty(name);
        }
    }
}
