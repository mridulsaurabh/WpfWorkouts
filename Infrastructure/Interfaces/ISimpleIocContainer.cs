using Infrastructure.Common;
using System;
namespace Infrastructure.Interfaces
{
    public interface ISimpleIocContainer
    {
        void RegisterType<TConcrete>();
        void RegisterType<TConcrete>(LifeCycle lifeCyle);
        void RegisterType<TTypeToResolve, TConcrete>();
        void RegisterType<TTypeToResolve, TConcrete>(LifeCycle lifeCycle);
        object Resolve(Type typeToResolve);
        TTypeToResolve Resolve<TTypeToResolve>();
    }
}
