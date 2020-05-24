using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Shinobytes.Ravenfall.RavenNet.Core
{
    public class IoC : IDisposable
    {
        private readonly ConcurrentDictionary<Type, object> instances
            = new ConcurrentDictionary<Type, object>();

        private readonly ConcurrentDictionary<Type, TypeLookup> typeLookup
            = new ConcurrentDictionary<Type, TypeLookup>();

        private readonly ConcurrentDictionary<Type, Func<object>> typeFactories
            = new ConcurrentDictionary<Type, Func<object>>();

        public void RegisterShared<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            typeLookup[typeof(TInterface)] = new TypeLookup(typeof(TImplementation), true);
        }
        public void RegisterShared(Type tInterface, Type tImplementation)
        {
            typeLookup[tInterface] = new TypeLookup(tImplementation, true);
        }
        public void Register<TInterface, TImplementation>()
        {
            typeLookup[typeof(TInterface)] = new TypeLookup(typeof(TImplementation), false);
        }

        public void Register(Type tInterface, Type implementation)
        {
            typeLookup[tInterface] = new TypeLookup(implementation, false);
        }

        public void Register<TImplementation>()
        {
            typeLookup[typeof(TImplementation)] = new TypeLookup(typeof(TImplementation), false);
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolve(typeof(TInterface));
        }

        public object Resolve(Type t)
        {
            var interfaceType = t;
            if (!typeLookup.TryGetValue(t, out var targetType))
                throw new Exception($"Unable to resolve the type {t.Name}");

            if (targetType.Shared)
            {
                if (instances.TryGetValue(t, out var obj))
                {
                    return obj;
                }
            }

            if (typeFactories.TryGetValue(interfaceType, out var factory))
            {
                var item = factory();
                instances[interfaceType] = item;
                return item;
            }

            var publicConstructors = targetType.Type
                .GetConstructors(BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance);

            foreach (var ctor in publicConstructors)
            {
                var param = ctor.GetParameters();
                if (param.Length == 0)
                {
                    var instance = ctor.Invoke(null);
                    if (targetType.Shared) instances[interfaceType] = instance;
                    return instance;
                }

                if (param.Any(x => x.ParameterType.IsValueType))
                {
                    // this constructor has a value type arguments
                    continue;
                }

                var item = ctor.Invoke(param.Select(x => Resolve(x.ParameterType)).ToArray());
                if (targetType.Shared) instances[interfaceType] = item;
                return item;
            }
            throw new Exception($"Unable to resolve the type {targetType.Type.Name}");
        }

        public void RegisterCustomShared<T>(Func<object> func)
        {
            typeLookup[typeof(T)] = new TypeLookup(typeof(T), true);
            typeFactories[typeof(T)] = func;
        }

        public void RegisterCustom<T>(Func<object> func)
        {
            typeLookup[typeof(T)] = new TypeLookup(typeof(T), false);
            typeFactories[typeof(T)] = func;
        }

        public void Dispose()
        {
            foreach (var instance in instances.Values)
            {
                if (instance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private class TypeLookup
        {
            public TypeLookup(Type type, bool shared)
            {
                Type = type;
                Shared = shared;
            }

            public Type Type { get; }
            public bool Shared { get; }
        }
    }
}
