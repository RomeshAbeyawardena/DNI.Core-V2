using DNI.Core.Contracts;
using DNI.Core.Domains;
using DNI.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class ServiceBrokerExtensions
    {
        public static IEnumerable<IServiceRegistration> GetServiceRegistrations(this IServiceBroker serviceBroker)
        {
            Func<Type, bool> findinterfaceExpression = type => type.GetInterfaces().Any(interfaceType => interfaceType == typeof(IServiceRegistration));
            var filteredAssemblies = serviceBroker.Assemblies
                .Where(a => a.GetTypes().Any(findinterfaceExpression));

            foreach (var assembly in filteredAssemblies)
            {
                foreach(var type in assembly.GetTypes().Where(findinterfaceExpression))
                {
                    if(type == typeof(ServiceRegistration))
                        continue;

                    yield return Activator.CreateInstance(type) as IServiceRegistration;
                }
            }
        }
    }
}
