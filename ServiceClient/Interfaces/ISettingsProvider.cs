using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Interfaces
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
