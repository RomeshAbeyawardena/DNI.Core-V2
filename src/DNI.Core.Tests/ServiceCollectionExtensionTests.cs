using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Builders;
using DNI.Core.Shared.Extensions;
using DNI.Core.Services.Extensions;
using DNI.Core.Services.Implementations.Generators;
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
            var valueGeneratorProvider = serviceProvider.GetService<IValueGeneratorManager>();
            
            var encryptionProfileManager = serviceProvider.GetService<IEncryptionProfileManager>();

            var mapperProvider = serviceProvider.GetService<IMapperProvider>();
            var mediatorProvider = serviceProvider.GetService<IMediatorProvider>();

            Assert.IsNotNull(valueGeneratorProvider);
            Assert.IsNull(encryptionProfileManager);
            
            Assert.IsNull(mapperProvider);
            Assert.IsNull(mediatorProvider);

            Assert.True(valueGeneratorProvider.TryGetValue(Shared.Constants.Generators.DateTimeOffSetValueGenerator, out var dateTimeOffSetValueGenerator));
            Assert.IsNotNull(dateTimeOffSetValueGenerator);
            Assert.AreEqual(dateTimeOffSetValueGenerator.GetType().GetUnderlyingSystemType(dateTimeOffSetValueGenerator), typeof(DateTimeOffSetValueGenerator));

            Assert.True(valueGeneratorProvider.TryGetValue(Shared.Constants.Generators.DateTimeValueGenerator, out var dateTimeValueGenerator));
            Assert.IsNotNull(dateTimeValueGenerator);
            Assert.AreEqual(dateTimeOffSetValueGenerator.GetType().GetUnderlyingSystemType(dateTimeValueGenerator), typeof(DateTimeValueGenerator));

            Assert.True(valueGeneratorProvider.TryGetValue(Shared.Constants.Generators.GuidValueGenerator, out var guidValueGenerator));
            Assert.IsNotNull(guidValueGenerator);
            Assert.AreEqual(dateTimeOffSetValueGenerator.GetType().GetUnderlyingSystemType(guidValueGenerator), typeof(GuidValueGenerator));

            //Clear down for new test
            SetUp();

            Services.Extensions.ServiceCollectionExtensions.RegisterServices(services, 
                (serviceProvider, builder) => builder.Add(EncryptionClassification.Personal, encryptionProfileBuilder));
            serviceProvider = services.BuildServiceProvider();

             encryptionProfileManager = serviceProvider.GetService<IEncryptionProfileManager>();

            Assert.IsNotNull(encryptionProfileManager);
            Assert.IsTrue(encryptionProfileManager.TryGetValue(EncryptionClassification.Personal, out var actualEncryptionProfile));
            Assert.IsNotNull(actualEncryptionProfile);
        }

        private IEncryptionProfile encryptionProfileBuilder(IEncryptionProfile encryptionProfile)
        {
            encryptionProfile.Key = Array.Empty<byte>();
            return encryptionProfile;
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
            Services.Extensions.ServiceCollectionExtensions.RegisterServices(services, (serviceProvider, builder) => builder.Add(EncryptionClassification.Personal, encryptionProfileBuilder));

            Services.Extensions.ServiceCollectionExtensions.RegisterAutoMapperProviders(services, assembly => assembly.GetAssembly<ServiceCollectionExtensionTests
                >());

            var serviceProvider = services.BuildServiceProvider();

            var modelEncryptionProvider = serviceProvider.GetService<IModelEncryptionProvider>();

            Assert.IsNotNull(modelEncryptionProvider);
        }

        private ServiceCollection services;
    }

}
