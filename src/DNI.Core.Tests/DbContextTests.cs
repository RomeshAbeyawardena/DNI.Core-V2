using Castle.Components.DictionaryAdapter;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Domains;
using DNI.Core.Services.Implementations.Generators;
using DNI.Core.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;

namespace DNI.Core.Tests
{
    public class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        [Encrypt(Shared.Enumerations.EncryptionMethod.TwoWay, Shared.Enumerations.EncryptionClassification.Personal)]
        public string EmailAddress { get; set; }

        [GeneratedDefaultValue(nameof(DateTimeOffSetValueGenerator))]
        public DateTimeOffset Created { get; set; }
    }

    public class DbContextTests
    {
        private Mock<IValueGeneratorProvider> valueGeneratorProviderMock;
        private Mock<IValueGenerator> valueGeneratorMock;
        private TestDbContext sut;
        private DbContextOptions dbContextOptions;

        [SetUp]
        public void Setup()
        {
            valueGeneratorProviderMock = new Mock<IValueGeneratorProvider>();
            valueGeneratorMock = new Mock<IValueGenerator>();

            valueGeneratorProviderMock.Setup(sut => sut.GetValueGeneratorByName(nameof(DateTimeOffSetValueGenerator), true))
                .Returns(valueGeneratorMock.Object)//DateTimeOffSetValueGenerator
                .Verifiable();

            dbContextOptions = DbContextOptionsTestBuilder
                .Build(services => services
                    .AddSingleton(valueGeneratorProviderMock.Object)
                    .AddSingleton<IRepositoryOptions>(new RepositoryOptions()));

            sut = new TestDbContext(dbContextOptions);
        }

        [Test]
        public void EnhancedDbContextBase()
        {
            var expectedDateTimeOffset = new DateTimeOffset(new DateTime(2020, 09, 20, 15, 30, 0), TimeSpan.FromHours(-1d));
            valueGeneratorMock.Setup(sut => sut.GenerateValue)
                .Returns((a) => expectedDateTimeOffset)
                .Verifiable();

            var testUser = new User();

            var entry = sut.Entry(testUser);
            sut.ReportChange(entry);
            valueGeneratorProviderMock.Verify();
            valueGeneratorMock.Verify();
            Assert.AreEqual(expectedDateTimeOffset, testUser.Created);
        }
    }
}