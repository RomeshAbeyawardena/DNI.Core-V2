using DNI.Core.Contracts;
using DNI.Core.Contracts.Collectors;
using DNI.Core.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DNI.Core.Services.Extensions
{
    public static class DefinitionExtensions
    {
        public static IEnumerable<Type> CollectServices<TService>(this IDefinition<Assembly> descriptor, ITypeCollector typeCollectorInstance = null)
        {
            if(descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if(typeCollectorInstance == null)
            {
                typeCollectorInstance = TypeCollector.Default;
            }

            return typeCollectorInstance.Collect<TService>(descriptor.Definitions.ToArray());
        }

        public static IEnumerable<Type> CollectServices<TService>(this IDefinition<Type> descriptor, ITypeCollector typeCollectorInstance = null)
        {
            if(descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if(typeCollectorInstance == null)
            {
                typeCollectorInstance = TypeCollector.Default;
            }

            return typeCollectorInstance.Collect<TService>(descriptor.Definitions.ToArray());
        }

        public static IDefinition<Type> DescribeType<T>(this IDefinition<Type> descriptor)
        {
            if(descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            return descriptor.Add(typeof(T));
        }

        public static IDefinition<Assembly> DescribeAssembly<T>(this IDefinition<Assembly> descriptor)
        {
            if(descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            return descriptor.Add(Assembly.GetAssembly(typeof(T)));
        }
    }
}
