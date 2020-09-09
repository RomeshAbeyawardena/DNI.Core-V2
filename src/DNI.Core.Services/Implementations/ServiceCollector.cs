using DNI.Core.Contracts;
using DNI.Core.Services.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations
{
    public class ServiceCollector : IServiceCollector
    {
        public static IServiceCollector Default => new ServiceCollector();

        public IEnumerable<Type> Collect<TService>(Action<IDefinition<Assembly>> describeAssemblies)
        {
            var assemblyDefinition = new AssemblyDefinition();
            describeAssemblies(assemblyDefinition);
            return Collect<TService>(assemblyDefinition.Definitions.ToArray());
        }

        public IEnumerable<Type> Collect(Type serviceType, params Assembly[] assemblies)
        {
            var collectedServiceList = new List<Type>();
            foreach(var assembly in assemblies)
            {
                collectedServiceList.AddRange(Collect(serviceType, assembly.GetTypes()));
            }

            return collectedServiceList;
        }

        public IEnumerable<Type> Collect<TService>(params Assembly[] assemblies)
        {
            var serviceType = typeof(TService);

            if (!serviceType.IsInterface)
            {
                throw new ArgumentException("Expected interface as instance type", nameof(TService));
            }

            return Collect(serviceType, assemblies);
        }

        public IEnumerable<Type> Collect(Type serviceType, params Type[] serviceTypes)
        {
            return serviceTypes.Where(type => type.IsAbstract == false
                && type.IsClass 
                && (type.IsAssignableFrom(serviceType)
                    || type.GetInterfaces().Any(@interface => @interface == serviceType)));
        }
    }
}
