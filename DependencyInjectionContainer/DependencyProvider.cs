using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionContainer
{
    public class DependencyProvider
    {
        public DependencyProvider(DependenciesConfigurator configurations)
        {
            RegisteredConfigurations = configurations.RegisteredConfigurations;
        }

        private Dictionary<Type, List<ImplementationConfiguration>> RegisteredConfigurations { get; }

        private ConcurrentDictionary<Type, object> ImplementationInstances { get; } =
            new ConcurrentDictionary<Type, object>();

        private object Resolve(Type tDependency, int implementationId = 0)
        {
            if (typeof(IEnumerable).IsAssignableFrom(tDependency)) // tDependency is IEnumerable<T>
            {
                var actualDependency = tDependency.GetGenericArguments()[0];
                var implementationsCount = RegisteredConfigurations[actualDependency].Count;
                var container = Array.CreateInstance(actualDependency, implementationsCount);

                for (var i = 0; i < implementationsCount; i++)
                    container.SetValue(Resolve(actualDependency, i), i);
                return container;
            }

            var isGenericDependency = tDependency.GenericTypeArguments.Length == 0 ? false : true;

            if (!isGenericDependency && !RegisteredConfigurations.ContainsKey(tDependency))
                throw new ArgumentOutOfRangeException(string.Format("TDependency of type {0} was not registered",
                    tDependency.Name));
            if (!isGenericDependency && implementationId >= RegisteredConfigurations[tDependency].Count)
                throw new ArgumentOutOfRangeException("Specified named implementation not found");

            ImplementationConfiguration implConfig;
            var IsDependencyOpenGeneric = false;
            if (isGenericDependency)
            {
                var t = tDependency.GetGenericTypeDefinition();
                if (RegisteredConfigurations.ContainsKey(t))
                {
                    implConfig = RegisteredConfigurations[t][0]; // open generics
                    IsDependencyOpenGeneric = true;
                }
                else
                {
                    implConfig = RegisteredConfigurations[tDependency][0]; //generics
                }
            }
            else
            {
                implConfig = RegisteredConfigurations[tDependency][implementationId];
            }

            var targetType = implConfig.ImplementationType;
            if (IsDependencyOpenGeneric)
                targetType = targetType.MakeGenericType(tDependency.GetGenericArguments()[0]);

            if (ImplementationInstances.ContainsKey(targetType))
                return ImplementationInstances[targetType];

            var ctor = targetType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
            var ctorParamInfos = ctor.GetParameters();
            var ctorParams = new object[ctorParamInfos.Length];

            for (var i = 0; i < ctorParams.Length; i++)
                if (ctorParamInfos[i].ParameterType.IsValueType)
                {
                    ctorParams[i] = Activator.CreateInstance(ctorParamInfos[i].ParameterType);
                }
                else
                {
                    if (isGenericDependency)
                    {
                        ctorParams[i] = Resolve(ctorParamInfos[i].ParameterType, implementationId);
                    }
                    else
                    {
                        var a = ctorParamInfos[i].GetCustomAttribute(typeof(DependencyKeyAttribute));
                        if (a is DependencyKeyAttribute key)
                            ctorParams[i] = Resolve(ctorParamInfos[i].ParameterType, Convert.ToInt32(key.Name));
                        else ctorParams[i] = Resolve(ctorParamInfos[i].ParameterType);
                    }
                }

            try
            {
                var result = ctor.Invoke(ctorParams);
                if (implConfig.ImplementationLifetime == DependenciesConfigurator.Lifetime.Singleton)
                    return ImplementationInstances.TryAdd(targetType, result)
                        ? result
                        : ImplementationInstances[targetType];
                return result;
            }
            catch
            {
                throw new ArgumentException(targetType.Name + " constructor threw an exception");
            }
        }

        public TDependency Resolve<TDependency>(Enum namedImplementation = null) where TDependency : class
        {
            return (TDependency) Resolve(typeof(TDependency), Convert.ToInt32(namedImplementation));
        }
    }
}