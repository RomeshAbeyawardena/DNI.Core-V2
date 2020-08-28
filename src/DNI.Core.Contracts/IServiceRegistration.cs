using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}
