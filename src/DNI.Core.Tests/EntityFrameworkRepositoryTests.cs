using DNI.Core.Contracts;
using DNI.Core.Services;
using DNI.Core.Services.Implementations.Data;
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
    public class EntityFrameworkRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            dbContextOptions = DbContextOptionsTestBuilder
                .Build(services => services);
            testDbContext = new TestDbContext(dbContextOptions);
            sut = new EntityFrameworkRepository<TestDbContext, User>(testDbContext, RepositoryOptionsBuilder.Build(RepositoryOptionsBuilder.Default));
        }

        [Test]
        public void SaveChanges()
        {
            var user = new User();
            sut.SaveChanges(user);
            Assert.AreEqual(EntityState.Added, sut.LastEntityState);
            Assert.AreNotEqual(default(int), user.Id);
            user.Created = DateTimeOffset.Now;

            sut.SaveChanges(user);
            Assert.AreEqual(EntityState.Modified, sut.LastEntityState);

        }

        private TestDbContext testDbContext;
        private DbContextOptions dbContextOptions;
        private EntityFrameworkRepository<TestDbContext, User> sut;
    }
}
