using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Extensions;
using DNI.Core.Services.Providers;
using DNI.Core.Shared.Enumerations;
using DNI.Core.Tests.Generators;
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
            
            Services.Extensions.ServiceCollectionExtensions
                .RegisterRepositories<TestDbContext>(services, (serviceProvider, options) => options.UseInMemoryDatabase("TestDb"));

            Assert.AreEqual(6, services.Count());

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
            var valueGeneratorProvider = serviceProvider.GetService<IValueGeneratorProvider>();
            var encryptionProfileManager = serviceProvider.GetService<IEncryptionProfileManager>();
            Assert.IsNotNull(valueGeneratorProvider);
            Assert.IsNull(encryptionProfileManager);

            //Clear down for new test
            SetUp();

            var encryptionProfile = 
                    EncryptionProfileBuilder
                        .BuildProfile(profile => profile.Key = Array.Empty<byte>() );

            Assert.IsNotNull(encryptionProfile.Key);

            Services.Extensions.ServiceCollectionExtensions.RegisterServices(services, 
                (builder, serviceProvider) => builder.Add(EncryptionClassification.Personal, encryptionProfile));
            serviceProvider = services.BuildServiceProvider();

             encryptionProfileManager = serviceProvider.GetService<IEncryptionProfileManager>();

            Assert.IsNotNull(encryptionProfileManager);
            Assert.IsTrue(encryptionProfileManager.TryGetValue(EncryptionClassification.Personal, out var actualEncryptionProfile));
            Assert.IsNotNull(actualEncryptionProfile);
        }

        [Test]
        public void ScanAndRegisterGenerators()
        {
            var valueGenerators = Services.Extensions.ServiceCollectionExtensions.ScanAndRegisterGenerators<TestValueGenerator>(services);

            Assert.AreEqual(2, valueGenerators.Count());
            Assert.AreEqual(2, services.Count());
        }

        [Test]
        public void RegisterAutoMapperProviders()
        {
            Services.Extensions.ServiceCollectionExtensions.RegisterAutoMapperProviders(services, assembly => assembly.GetAssembly<ServiceCollectionExtensionTests
                >());
        }

        private ServiceCollection services;
    }

}
