using Castle.Components.DictionaryAdapter;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Implementations.Generators;
using DNI.Core.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;

namespace DNI.Core.Tests
{
    public class TestDbContext : EnhancedDbContextBase
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        [GeneratedDefaultValue(nameof(DateTimeOffSetValueGenerator))]
        public DateTimeOffset Created { get; set; }
    }

    public class DbContextTests
    {
        private Mock<IValueGeneratorProvider> valueGeneratorProviderMock;
        private Mock<IValueGenerator> valueGeneratorMock;
        private TestDbContext sut;
        private DbContextOptions dbContextOptions;

        public IServiceProvider BuildServiceProvider(Func<IServiceCollection, IServiceCollection> services)
        {
            var serviceCollection = new ServiceCollection();
            return services(serviceCollection)
                .BuildServiceProvider();
        }

        [SetUp]
        public void Setup()
        {
            valueGeneratorProviderMock = new Mock<IValueGeneratorProvider>();
            valueGeneratorMock = new Mock<IValueGenerator>();

            valueGeneratorProviderMock.Setup(sut => sut.GetValueGeneratorByName(nameof(DateTimeOffSetValueGenerator), true))
                .Returns(valueGeneratorMock.Object)
                .Verifiable();
            
            dbContextOptions = new DbContextOptionsBuilder()
                .UseInternalServiceProvider(BuildServiceProvider(services => services
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddSingleton(valueGeneratorProviderMock.Object)))
                .UseInMemoryDatabase("TestDb").Options;
            sut = new TestDbContext(dbContextOptions);
        }

        [Test]
        public void EnhancedDbContextBase()
        {
            valueGeneratorMock.Setup(sut => sut.GenerateValue)
                .Returns((a) => new DateTimeOffset(new DateTime(2020, 09, 20, 15, 30, 0), TimeSpan.FromHours(-1d)))
                .Verifiable();

            sut.Add(new User());
            
            valueGeneratorProviderMock.Verify();
            valueGeneratorMock.Verify();
        }
    }
}