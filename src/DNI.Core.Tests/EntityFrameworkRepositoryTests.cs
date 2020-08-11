using DNI.Core.Contracts;
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
            sut = new EntityFrameworkRepository<TestDbContext, User>(testDbContext);
        }

        [Test]
        public void SaveChanges()
        {
            sut.SaveChanges(new User());
        }

        private TestDbContext testDbContext;
        private DbContextOptions dbContextOptions;
        private EntityFrameworkRepository<TestDbContext, User> sut;
    }
}
