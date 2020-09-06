using DNI.Core.Contracts;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DNI.Core.Tests
{
    public class TestServiceBroker : ServiceBroker
    {
        public TestServiceBroker() 
            : base(definitions => definitions.DescribeAssembly<ServiceBrokerTests>())
        {
        }
    }

    public class TestServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton("apple");
        }
    }

    public class TestServiceRegistration2 : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(new User { Id = 22 });
        }
    }


    [TestFixture]
    public class ServiceBrokerTests
    {
        [SetUp]
        public void SetUp()
        {
            services = new ServiceCollection();
        }

        [Test]
        public void RegisterServices()
        {
            Services.Extensions.ServiceCollectionExtensions.RegisterServiceBroker<TestServiceBroker>(services);
            Assert.IsTrue(services.Count == 2);
        }

        private IServiceCollection services;
    }
}
