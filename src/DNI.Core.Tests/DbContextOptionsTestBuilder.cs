using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Tests
{
    public static class DbContextOptionsTestBuilder
    {
        public static DbContextOptions Build(Func<IServiceCollection, IServiceCollection> serviceConfiguration)
        {
            return new DbContextOptionsBuilder()
                .UseInternalServiceProvider(BuildServiceProvider(services => serviceConfiguration(services
                    .AddEntityFrameworkInMemoryDatabase())))
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
