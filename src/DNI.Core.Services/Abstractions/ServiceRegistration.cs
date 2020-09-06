using DNI.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ServiceRegistration : IServiceRegistration
    {
        public abstract void RegisterServices(IServiceCollection services);
    }
}
