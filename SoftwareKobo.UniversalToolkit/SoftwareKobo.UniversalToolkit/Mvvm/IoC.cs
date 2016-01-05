using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public class IoC : Microsoft.Practices.ServiceLocation.ServiceLocatorImplBase
    {
        // 存放每个实际类型所使用的默认构造函数。
        private readonly Dictionary<Type, ConstructorInfo> _defaultConstructor = new Dictionary<Type, ConstructorInfo>();

        private readonly string _defaultKey = Guid.NewGuid().ToString();

        // 存储对应的 key 再对应 service type 的实例。
        private readonly Dictionary<string, Dictionary<Type, object>> _instances = new Dictionary<string, Dictionary<Type, object>>();

        // 映射接口到实际实现的类。
        private readonly Dictionary<Type, Type> _interfaceClassMap = new Dictionary<Type, Type>();

        // 每个 key 所注册的服务类型。对于 Register<TClass>，里面存的 Type 就是 TClass。对于 Register<TInterface,TClass>，里面存的 Type 就是 TInterface，然后再用 _interfaceClassMap 获取 TClass。
        private readonly Dictionary<string, List<Type>> _keyRegisteredTypes = new Dictionary<string, List<Type>>();

        public bool IsRegistered(Type serviceType)
        {
            return IsRegistered(serviceType, null);
        }

        public bool IsRegistered<TService>(string key) where TService : class
        {
            return IsRegistered(typeof(TService), key);
        }

        public bool IsRegistered(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            key = key ?? _defaultKey;

            if (_keyRegisteredTypes.ContainsKey(key) == false)
            {
                return false;
            }
            return _keyRegisteredTypes[key].Contains(serviceType);
        }

        public bool IsRegistered<TService>() where TService : class
        {
            return IsRegistered(typeof(TService));
        }

        public void Register(Type classType)
        {
            Register(classType, key: null);
        }

        public void Register(Type classType, string key)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            var classTypeInfo = classType.GetTypeInfo();
            if (classTypeInfo.IsClass == false || classTypeInfo.IsInterface)
            {
                throw new ArgumentException($"{nameof(classType)} could not be interface.", nameof(classType));
            }

            key = key ?? _defaultKey;

            if (IsRegistered(classType, key))
            {
                throw new ArgumentException("this classType had registered.", nameof(classType));
            }

            if (_keyRegisteredTypes.ContainsKey(key) == false)
            {
                _keyRegisteredTypes.Add(key, new List<Type>());
            }
            var registeredTypes = _keyRegisteredTypes[key];
            registeredTypes.Add(classType);
        }

        public void Register<TClass>() where TClass : class
        {
            Register<TClass>(null);
        }

        public void Register<TClass>(string key) where TClass : class
        {
            Register(typeof(TClass), key);
        }

        public void Register(Type interfaceType, Type classType)
        {
            Register(interfaceType, classType, null);
        }

        public void Register(Type interfaceType, Type classType, string key)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            var interfaceTypeInfo = interfaceType.GetTypeInfo();
            var classTypeInfo = classType.GetTypeInfo();

            if (interfaceTypeInfo.IsInterface == false)
            {
                throw new ArgumentException($"{nameof(interfaceType)} is not interface.", nameof(interfaceType));
            }
            if (classTypeInfo.IsClass == false || classTypeInfo.IsInterface)
            {
                throw new ArgumentException($"{nameof(classType)} must be a class and could not be interface.", nameof(classType));
            }

            key = key ?? _defaultKey;

            if (IsRegistered(interfaceType, key))
            {
                throw new ArgumentException("this interfaceType had registered.", nameof(interfaceType));
            }

            if (_interfaceClassMap.ContainsKey(interfaceType) && _interfaceClassMap[interfaceType] != classType)
            {
                throw new ArgumentException("this interface had registered and the implement type is not equal class type.", nameof(interfaceType));
            }

            if (_keyRegisteredTypes.ContainsKey(key) == false)
            {
                _keyRegisteredTypes.Add(key, new List<Type>());
            }
            var registeredTypes = _keyRegisteredTypes[key];
            registeredTypes.Add(interfaceType);
            _interfaceClassMap[interfaceType] = classType;
        }

        public void Register<TInterface, TClass>() where TClass : class, TInterface where TInterface : class
        {
            Register<TInterface, TClass>(null);
        }

        public void Register<TInterface, TClass>(string key) where TClass : class, TInterface where TInterface : class
        {
            Register(typeof(TInterface), typeof(TClass), key);
        }

        public void Unregister<TService>() where TService : class
        {
            Unregister(typeof(TService));
        }

        public void Unregister(Type serviceType)
        {
            Unregister(serviceType, null);
        }

        public void Unregister(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            key = key ?? _defaultKey;

            if (_keyRegisteredTypes.ContainsKey(key) == false)
            {
                return;
            }
            var registeredTypes = _keyRegisteredTypes[key];
            registeredTypes.Remove(serviceType);
        }

        public void Unregister<TService>(string key) where TService : class
        {
            Unregister(typeof(TService), null);
        }

        protected override sealed IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return InternalGetAllInstances(serviceType);
        }

        protected override sealed object DoGetInstance(Type serviceType, string key)
        {
            return InternalGetInstance(serviceType, key);
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            if (_defaultConstructor.ContainsKey(type))
            {
                // 返回已经缓存的构造函数。
                return _defaultConstructor[type];
            }

            // 最适合的构造函数。
            ConstructorInfo suitableConstructor;

            var constructors = type.GetConstructors();
            var preferredConstructors = constructors.Where(temp => temp.GetCustomAttribute<PreferredConstructorAttribute>() != null).ToList();
            if (preferredConstructors.Count == 1)
            {
                suitableConstructor = preferredConstructors.Single();
            }
            else if (preferredConstructors.Count > 1)
            {
                // 不能标注多个标签。
                throw new ArgumentException($"could not set two or more {nameof(PreferredConstructorAttribute)}", nameof(type));
            }
            else
            {
                // 获取无参构造函数。
                var defaultConstructor = constructors.Length == 1 ? constructors[0] : type.GetConstructor(Type.EmptyTypes);
                if (defaultConstructor == null)
                {
                    throw new ArgumentException("could not find a suitable constructor.", nameof(type));
                }
                suitableConstructor = defaultConstructor;
            }

            // 缓存起来。
            _defaultConstructor[type] = suitableConstructor;
            return suitableConstructor;
        }

        private IEnumerable<object> InternalGetAllInstances(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            foreach (var keyValue in _keyRegisteredTypes)
            {
                yield return InternalGetInstance(serviceType, keyValue.Key);
            }
        }

        private object InternalGetInstance(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            key = key ?? _defaultKey;

            if (_keyRegisteredTypes.ContainsKey(key) == false)
            {
                throw new ArgumentException("key is not registered.", nameof(key));
            }

            var registeredTypes = _keyRegisteredTypes[key];
            if (registeredTypes.Contains(serviceType) == false)
            {
                throw new ArgumentException("service type is not registered.", nameof(serviceType));
            }

            Type instanceType;
            if (serviceType.GetTypeInfo().IsInterface)
            {
                instanceType = _interfaceClassMap[serviceType];
            }
            else
            {
                instanceType = serviceType;
            }

            var constructor = GetConstructor(instanceType);
            var constructorParameterInfos = constructor.GetParameters();
            var parameters = new object[constructorParameterInfos.Length];
            for (var i = 0; i < constructorParameterInfos.Length; i++)
            {
                var parameterType = constructorParameterInfos[i].ParameterType;
                object parameter;
                parameter = InternalGetInstance(parameterType, key);
                if (parameter == null && key != _defaultKey)
                {
                    parameter = InternalGetInstance(parameterType, null);
                }
                parameters[i] = parameter;
            }

            var instance = constructor.Invoke(parameters);
            return instance;
        }
    }
}