using DNI.Core.Contracts;
using DNI.Core.Contracts.Collectors;
using DNI.Core.Services.Definitions;
using DNI.Core.Services.Extensions;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations
{
    [IgnoreScanning]
    public class TypeCollector : ITypeCollector
    {
        public static ITypeCollector Default => new TypeCollector();

        public static ITypeCollector Create(Expression<Func<Type, bool>> serviceFilter)
        {
            return new TypeCollector(serviceFilter);
        }

        private TypeCollector()
            : this(type => type.IsAbstract == false
                && type.IsClass)
        {

        }

        private TypeCollector(Expression<Func<Type, bool>> serviceFilter)
        {
            Filter = serviceFilter;
        }

        public Expression<Func<Type, bool>> Filter { get; }

        public IEnumerable<Type> Collect<TService>(Action<IDefinition<Assembly>> describeAssemblies)
        {
            var assemblyDefinition = AssemblyDefinition.Default;
            describeAssemblies(assemblyDefinition);
            return Collect<TService>(assemblyDefinition.Definitions.ToArray());
        }

        public IEnumerable<Type> Collect(Type serviceType, IEnumerable<Assembly> assemblies)
        {
            var collectedServiceList = new List<Type>();
            foreach(var assembly in assemblies)
            {
                collectedServiceList.AddRange(Collect(serviceType, assembly.GetTypes()));
            }

            return collectedServiceList;
        }

        public IEnumerable<Type> Collect<TService>(IEnumerable<Assembly> assemblies)
        {
            var serviceType = typeof(TService);

            if (!serviceType.IsInterface)
            {
                throw new ArgumentException("Expected interface as instance type", nameof(TService));
            }

            return Collect(serviceType, assemblies);
        }

        public IEnumerable<Type> Collect(Type serviceType, IEnumerable<Type> serviceTypes)
        {
            Expression<Func<Type, bool>> conditionExpression = type => (type.IsAssignableFrom(serviceType)
                    || type.GetInterfaces().Any(@interface => @interface == serviceType));

            var filter = (Filter == null) 
                ? conditionExpression.Compile()
                : Filter.Combine(conditionExpression);

            return serviceTypes.Where(filter);
        }

        public IEnumerable<Type> Collect<TService>(IEnumerable<Type> types)
        {
            return Collect(typeof(TService), types.ToArray());
        }

        public IEnumerable<Type> Collect<TService>(Action<IDefinition<Type>> describeTypes)
        {
            var typeDefinition = TypeDefinition.Default;
            describeTypes(typeDefinition);
            return Collect<TService>(typeDefinition.ToArray());
        }

    }
}
