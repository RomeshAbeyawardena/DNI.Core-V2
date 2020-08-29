using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class DefinitionExtensions
    {
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
