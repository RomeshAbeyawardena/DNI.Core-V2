using DNI.Core.Contracts;
using DNI.Core.Services.Extensions;
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
            services.AddScoped((serviceProvider) => DbContextOptionsTestBuilder.Build(services => services));
            services.AddScoped<TestDbContext>();
        }

        [Test]
        public void RegisterRepositories()
        {
            ServiceCollectionExtensions.RegisterRepositories<TestDbContext>(services);

            var serviceProvider = services.BuildServiceProvider();

            var repository = serviceProvider.GetRequiredService<IRepository<User>>();
            Assert.IsNotNull(repository);

            var asyncRepository = serviceProvider.GetRequiredService<IAsyncRepository<User>>();
            Assert.IsNotNull(asyncRepository);
        }

        private ServiceCollection services;
    }
}
