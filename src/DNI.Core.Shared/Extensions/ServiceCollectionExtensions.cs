using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void TryAdd(this IServiceCollection services, Type type, ServiceLifetime serviceLifetime)
        {
            services.TryAdd(type, type, serviceLifetime);
        }

        public static void TryAdd(this IServiceCollection services, Type type, Type implementationType, ServiceLifetime serviceLifetime)
        {
            services.TryAdd(ServiceDescriptor.Describe(type, implementationType, serviceLifetime));
        }
    }
}
