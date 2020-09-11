using DNI.Core.Contracts;
using DNI.Core.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Services.Extensions
{
    public static class ServiceBrokerExtensions
    {
        public static IEnumerable<IServiceRegistration> GetServiceRegistrations(this IServiceBroker serviceBroker)
        {
            var serviceRegistrationTypes = TypeCollector.Default.Collect<IServiceRegistration>(serviceBroker.Assemblies.ToArray());

            foreach(var serviceRegistrationType in serviceRegistrationTypes)
            {
                yield return Activator.CreateInstance(serviceRegistrationType) as IServiceRegistration;
            }
        }
    }
}
