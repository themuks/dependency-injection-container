using System;

namespace DependencyInjectionContainer
{
    internal class ImplementationConfiguration
    {
        internal ImplementationConfiguration(Type implementationType,
            DependenciesConfigurator.Lifetime implementationLifetime)
        {
            ImplementationType = implementationType;
            ImplementationLifetime = implementationLifetime;
        }

        internal Type ImplementationType { get; }
        internal DependenciesConfigurator.Lifetime ImplementationLifetime { get; }

        public override bool Equals(object obj)
        {
            if (obj is ImplementationConfiguration ic)
                return ic.ImplementationType == ImplementationType &&
                       ic.ImplementationLifetime == ImplementationLifetime;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + ImplementationType.GetHashCode();
            hash = hash * 23 + ImplementationLifetime.GetHashCode();
            return hash;
        }
    }
}