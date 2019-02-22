using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class IocContainer : IIocContainer
    {
        private readonly Dictionary<MappingKey, Func<object>> mappings;

        public IocContainer()
        {
            mappings = new Dictionary<MappingKey, Func<object>>();
        }

        public void Register<TFrom, TTo>(string instanceName = null)
        {
            Register(typeof(TFrom), typeof(TTo), instanceName);
        }

        public void Register(Type from, Type to, string instanceName = null)
        {
            if (to == null)
                throw new ArgumentNullException();

            if (!from.IsAssignableFrom(to))
            {
                string errorMessage = string.Format("Error trying to register the instance: '{0}' is not assignable from '{1}'",
                    from.FullName, to.FullName);

                throw new InvalidOperationException(errorMessage);
            }
            Func<object> createInstanceDelegate = () => Activator.CreateInstance(to);
            Register(from, createInstanceDelegate, instanceName);
        }

        public void RegisterInstance<T>(string instanceName =null)
        {
            RegisterInstance(typeof(T), instanceName);
        }

        public void RegisterInstance(Type type, string instanceName = null)
        {
            Register(type, type, instanceName);
        }
        
        public void Register(Type type, Func<object> createInstanceDelegate, string instanceName = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (createInstanceDelegate == null)
                throw new ArgumentNullException("createInstanceDelegate");


            var key = new MappingKey(type, instanceName);

            if (mappings.ContainsKey(key))
            {
                const string errorMessageFormat = "The requested mapping already exists - {0}";
                throw new InvalidOperationException(string.Format(errorMessageFormat, key.ToTraceString()));
            }

            mappings.Add(key, createInstanceDelegate);
        }

        public bool IsRegistered<T>(string instanceName = null)
        {
            return IsRegistered(typeof(T), instanceName);
        }

        public bool IsRegistered(Type type, string instanceName = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var key = new MappingKey(type, instanceName);
            return mappings.ContainsKey(key);
        }

        public T Resolve<T>(string instanceName = null)
        {
            object instance = Resolve(typeof(T), instanceName);
            return (T)instance;
        }

        public object Resolve(Type type, string instanceName = null)
        {
            var key = new MappingKey(type, instanceName);
            Func<object> createInstance;

            if (mappings.TryGetValue(key, out createInstance))
            {
                var instance = createInstance();
                return instance;
            }

            const string errorMessageFormat = "Could not find mapping for type '{0}'";
            throw new InvalidOperationException(string.Format(errorMessageFormat, type.FullName));
        }

        public override string ToString()
        {
            if (mappings == null)
                return "No mappings";

            return string.Join(Environment.NewLine, this.mappings.Keys);
        }

    }

    internal class MappingKey
    {
        public MappingKey(Type type, string instanceName)
        {
            if (type == null)
                throw new ArgumentNullException();

            Type = type;
            InstanceName = instanceName;
        }

        public Type Type { get; protected set; }

        public string InstanceName { get; protected set; }

        public override int GetHashCode()
        {
            unchecked
            {
                const int multiplier = 31;
                int hash = GetType().GetHashCode();

                hash = hash * multiplier + Type.GetHashCode();
                hash = hash * multiplier + (InstanceName == null ? 0 : InstanceName.GetHashCode());

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MappingKey compareTo = obj as MappingKey;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (compareTo == null)
                return false;

            return Type.Equals(compareTo.Type) &&
                string.Equals(InstanceName, compareTo.InstanceName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            const string format = "{0} ({1}) - hash code: {2}";

            return string.Format(format, this.InstanceName ?? "[null]",
                this.Type.FullName,
                this.GetHashCode()
            );
        }

        public string ToTraceString()
        {
            const string format = "Instance Name: {0} ({1})";

            return string.Format(format, this.InstanceName ?? "[null]",
                this.Type.FullName
            );
        }
    }

    public class SimpleContainer : ISimpleContainer
    {
        private readonly Dictionary<Type, Type> _dependencyMap = new Dictionary<Type, Type>();
        
        public void RegisterInstance<T>()
        {
            RegisterInsance(typeof(T));
        }

        public void RegisterInsance(Type type)
        {
            Register(type, type);
        }

        public void RegisterInstance(object instance)
        {

        }
        
        public void Register<TFrom, TTo>()
        {
            Register(typeof(TFrom), typeof(TTo));
        }

        public void Register(Type from, Type to)
        {
            _dependencyMap.Add(from, to);
        }

        public bool IsRegistered<T>()
        {
           return IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type type)
        {
            return this._dependencyMap.ContainsKey(type);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object Resolve(Type type)
        {
            Type resolvedType;
            if (_dependencyMap.TryGetValue(type, out resolvedType))
            {
                ConstructorInfo constructor = resolvedType.GetConstructors().First();
                TypeInfo info = resolvedType.GetTypeInfo();
                ParameterInfo[] parameters = constructor.GetParameters();

                if (!parameters.Any())
                {
                    return Activator.CreateInstance(resolvedType);
                }
                else
                {
                    return constructor.Invoke(ResolveParameters(parameters).ToArray());
                }
            }
            else
            {
                return new ArgumentNullException();
            }
        }

        private IEnumerable<object> ResolveParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters
                .Select(p => Resolve(p.ParameterType))
                .ToList();
        }
    }
}
