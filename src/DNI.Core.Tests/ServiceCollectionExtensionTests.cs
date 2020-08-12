using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Extensions;
using DNI.Core.Services.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class ServiceCollectionExtensionTests
    {

        [SetUp]
        public void SetUp()
        {
            services = new ServiceCollection();
        }

        [Test]
        public void RegisterRepositories()
        {
            services.AddScoped((serviceProvider) => DbContextOptionsTestBuilder.Build(services => services));
            services.AddScoped<TestDbContext>();
            Services.Extensions.ServiceCollectionExtensions.RegisterRepositories<TestDbContext>(services);

            Assert.AreEqual(5, services.Count());

            var serviceProvider = services.BuildServiceProvider();

            var repository = serviceProvider.GetRequiredService<IRepository<User>>();
            Assert.IsNotNull(repository);

            var asyncRepository = serviceProvider.GetRequiredService<IAsyncRepository<User>>();
            Assert.IsNotNull(asyncRepository);
        }

        [Test]
        public void RegisterServices()
        {
            Services.Extensions.ServiceCollectionExtensions.RegisterServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var valueGeneratorProvider = serviceProvider.GetRequiredService<IValueGeneratorProvider>();

            Assert.IsNotNull(valueGeneratorProvider);
        }


        private ServiceCollection services;
    }

}
