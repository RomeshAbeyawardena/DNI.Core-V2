using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Tests
{
    public static class DbContextOptionsTestBuilder
    {
        public static DbContextOptions Build(Func<IServiceCollection, IServiceCollection> serviceConfiguration, out IServiceProvider serviceProvider)
        {
            serviceProvider = BuildServiceProvider(services => serviceConfiguration(services
                    .AddEntityFrameworkInMemoryDatabase()));
            return new DbContextOptionsBuilder()
                .UseInternalServiceProvider(serviceProvider)
                .UseInMemoryDatabase("TestDb").Options;
        }

        
        public static IServiceProvider BuildServiceProvider(Func<IServiceCollection, IServiceCollection> services)
        {
            var serviceCollection = new ServiceCollection();
            return services(serviceCollection)
                .BuildServiceProvider();
        }

    }
}
