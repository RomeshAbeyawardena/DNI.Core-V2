using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Domains
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}
