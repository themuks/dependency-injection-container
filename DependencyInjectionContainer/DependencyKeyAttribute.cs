using System;

namespace DependencyInjectionContainer
{
    public class DependencyKeyAttribute : Attribute
    {
        public DependencyKeyAttribute(object namedImplementation)
        {
            Name = namedImplementation as Enum;
        }

        public Enum Name { get; }
    }
}