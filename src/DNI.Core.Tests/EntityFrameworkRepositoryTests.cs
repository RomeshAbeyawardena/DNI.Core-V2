using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services;
using DNI.Core.Services.Implementations.Data;
using DNI.Core.Services.Implementations.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class EntityFrameworkRepositoryTests
    {
        [SetUp]
        public void SetUp()
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
            testDbContext = new TestDbContext(dbContextOptions);
            sut = new EntityFrameworkRepository<TestDbContext, User>(testDbContext, RepositoryOptionsBuilder.Build(RepositoryOptionsBuilder.Default));
        }

        [Test]
        public void SaveChanges()
        {
            var expectedDateTimeOffset = new DateTimeOffset(new DateTime(2020, 09, 20, 15, 30, 0), TimeSpan.FromHours(-1d));
            
            valueGeneratorMock.Setup(sut => sut.GenerateValue)
                .Returns((a) => expectedDateTimeOffset)
                .Verifiable();

            var user = new User();
            sut.SaveChanges(user);
            Assert.AreEqual(EntityState.Added, sut.LastEntityState);
            Assert.AreNotEqual(default(int), user.Id);
            user.Created = DateTimeOffset.Now;

            sut.SaveChanges(user);
            Assert.AreEqual(EntityState.Modified, sut.LastEntityState);

        }

        private Mock<IValueGenerator> valueGeneratorMock;
        private Mock<IValueGeneratorProvider> valueGeneratorProviderMock;
        private TestDbContext testDbContext;
        private DbContextOptions dbContextOptions;
        private EntityFrameworkRepository<TestDbContext, User> sut;
    }
}
