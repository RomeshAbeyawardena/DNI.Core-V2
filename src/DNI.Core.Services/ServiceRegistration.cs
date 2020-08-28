using DNI.Core.Domains;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Services
{
    public abstract class ServiceRegistration : IServiceRegistration
    {
        public abstract void RegisterServices(IServiceCollection services);
    }
}
