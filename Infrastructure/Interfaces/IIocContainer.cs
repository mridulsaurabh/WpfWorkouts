using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IIocContainer
    {
        void Register<TFrom, TTo>(string instanceName = null);

        void Register(Type from, Type to, string instanceName = null);

        void Register(Type type, Func<object> createInstanceDelegate, string instanceName = null);

        bool IsRegistered<T>(string instanceName = null);

        bool IsRegistered(Type type, string instanceName = null);
        
        T Resolve<T>(string instanceName = null);

        object Resolve(Type type, string instanceName = null);

    }
}
