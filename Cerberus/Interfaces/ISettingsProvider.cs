using System.Collections.Generic;
using System.Reflection;

namespace Cerberus.Interfaces
{
    public interface ISettingsProvider
    {
        IEnumerable<PropertyInfo> GetAllProperties();
        PropertyInfo GetProperty(string name);
        object GetSetting(string key);
        T GetSetting<T>(string key);
        void Reset();
        void Save();
        void SetSetting<T>(string key, T value);
    }
}
