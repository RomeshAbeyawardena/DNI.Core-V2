using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IServiceBroker
    {
        void RegisterServices(IServiceCollection services, IEnumerable<IServiceRegistration> serviceRegistrations);
        IEnumerable<Assembly> Assemblies { get; }
    }
}
