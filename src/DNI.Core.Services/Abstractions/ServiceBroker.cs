using DNI.Core.Contracts;
using DNI.Core.Services.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ServiceBroker : IServiceBroker
    {
        public ServiceBroker(Action<IDefinition<Assembly>> defineAssemblyDefinitions)
        {
            var assemblyDefinitions = new AssemblyDefinition();
            defineAssemblyDefinitions(assemblyDefinitions);
            Assemblies = assemblyDefinitions.Definitions;
        }

        public void RegisterServices(IServiceCollection services, IEnumerable<IServiceRegistration> serviceRegistrations)
        {
            foreach(var serviceRegistration in serviceRegistrations)
            {
                serviceRegistration.RegisterServices(services);
            }
        }

        public IEnumerable<Assembly> Assemblies { get; }
    }
}
