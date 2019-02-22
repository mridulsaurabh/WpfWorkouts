using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class SimpleIocContainer : ISimpleIocContainer
    {
        private readonly IList<RegisteredObject> registeredObjects = new List<RegisteredObject>();

        public void RegisterType<TTypeToResolve, TConcrete>()
        {
            RegisterType<TTypeToResolve, TConcrete>(LifeCycle.Transient);
        }

        public void RegisterType<TTypeToResolve, TConcrete>(LifeCycle lifeCycle)
        {
            var regItem = new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), lifeCycle);
            Register(regItem);
        }

        public void RegisterType<TConcrete>()
        {
            RegisterType<TConcrete>(LifeCycle.Transient);
        }

        public void RegisterType<TConcrete>(LifeCycle lifeCyle)
        {
            RegisterType<TConcrete, TConcrete>(lifeCyle);
        }

        public void RegisterInstance<TTypeToResolve, TConcrete>(object concreteInstance)
        {
            var regItem = new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), concreteInstance);
            Register(regItem);
        }

        public void RegisterInstance<TConcrete>(object concreteInstance)
        {
            RegisterInstance<TConcrete, TConcrete>(concreteInstance);
        }

        private void Register(RegisteredObject regItem)
        {
            if (!this.registeredObjects.Any(t => t.TypeToResolve == regItem.TypeToResolve && t.ConcreteType == regItem.ConcreteType))
            {
                this.registeredObjects.Add(regItem);
            }
            else
            {
                // Do Nothing since type has been already registered so override the new registry.
                var existingMember = this.registeredObjects.FindMatch(t => t.TypeToResolve == regItem.TypeToResolve && t.ConcreteType == regItem.ConcreteType);
                if (existingMember != null)
                {
                    this.registeredObjects.Remove(existingMember);
                    this.registeredObjects.Add(regItem);
                }
            }
        }

        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            return (TTypeToResolve)ResolveObject(typeof(TTypeToResolve));
        }

        public object Resolve(Type typeToResolve)
        {
            return ResolveObject(typeToResolve);
        }

        private object ResolveObject(Type typeToResolve)
        {
            var registeredObject = this.registeredObjects.FirstOrDefault(o => o.TypeToResolve == typeToResolve);
            if (registeredObject == null)
            {
                throw new Exception(string.Format("The type {0} has not been registered", typeToResolve.Name));
            }
            return GetInstance(registeredObject);
        }

        private object GetInstance(RegisteredObject registeredObject)
        {
            if (registeredObject.Instance == null ||
                registeredObject.LifeCycle == LifeCycle.Transient)
            {
                var parameters = ResolveConstructorParameters(registeredObject);
                registeredObject.CreateInstance(parameters.ToArray());
            }
            return registeredObject.Instance;
        }

        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
        {
            ConstructorInfo constructorInfo = registeredObject.ConcreteType.GetConstructors().First();
            foreach (var parameter in constructorInfo.GetParameters())
            {
                if (parameter.ParameterType == typeof(string))
                    yield return string.Empty;
                else
                    yield return ResolveObject(parameter.ParameterType);
            }
        }
    }

    public class RegisteredObject
    {
        public RegisteredObject(Type typeToResolve, Type typeConcrete, LifeCycle lifeCycle)
        {
            this.TypeToResolve = typeToResolve;
            this.ConcreteType = typeConcrete;
            this.LifeCycle = lifeCycle;
        }

        public RegisteredObject(Type typeToResolve, Type typeConcrete, object instance, LifeCycle lifeCycle = LifeCycle.Singleton)
        {
            this.TypeToResolve = typeToResolve;
            this.ConcreteType = typeConcrete;
            this.Instance = instance;
            this.LifeCycle = lifeCycle;
        }

        public Type TypeToResolve { get; private set; }
        public Type ConcreteType { get; private set; }
        public object Instance { get; private set; }
        public LifeCycle LifeCycle { get; private set; }

        public void CreateInstance(object[] parameters)
        {
            this.Instance = Activator.CreateInstance(this.ConcreteType, parameters);
        }
    }

    public enum LifeCycle
    {
        Transient,
        Singleton
    }
}
