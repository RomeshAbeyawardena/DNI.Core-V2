using Microsoft.Extensions.DependencyInjection;

namespace DNI.Core.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}
