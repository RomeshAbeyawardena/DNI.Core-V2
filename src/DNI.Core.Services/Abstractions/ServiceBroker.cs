﻿using DNI.Core.Contracts;
using DNI.Core.Domains;
using DNI.Core.Services.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ServiceBroker : IServiceBroker
    {
        public ServiceBroker(Action<IAssemblyDefinition> defineAssemblyDefinitions)
        {
            var assemblyDefinitions = new AssemblyDefinition();
            defineAssemblyDefinitions(assemblyDefinitions);
            Assemblies = assemblyDefinitions.Assemblies;
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
