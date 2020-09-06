using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Contracts
{
    public interface IServiceBroker
    {
        void RegisterServices(IServiceCollection services, IEnumerable<IServiceRegistration> serviceRegistrations);
        IEnumerable<Assembly> Assemblies { get; }
    }
}
