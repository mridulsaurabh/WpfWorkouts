using System;
namespace Infrastructure.Interfaces
{
    public interface ISimpleContainer
    {
        bool IsRegistered(Type type);
        bool IsRegistered<T>();
        void Register(Type from, Type to);
        void Register<TFrom, TTo>();
        void RegisterInsance(Type type);
        void RegisterInstance<T>();
        T Resolve<T>();
    }
}
