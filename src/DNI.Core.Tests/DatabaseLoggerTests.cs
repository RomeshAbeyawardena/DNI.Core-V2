using DNI.Core.Contracts;
using DNI.Core.Domains;
using DNI.Core.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public int LogLevelId { get; set; }
        public string EventId { get; set; }
        public string State { get; set; }
        public string Message { get; set; }
        public string FormattedMessage { get; set; }
    }

    public class LogStatus
    {
        [Key]
        public int Id { get; set; }
        public int Level { get; set; }
        public bool Active { get; set; }
    }

    [TestFixture]
    public class DatabaseLoggerTests
    {

        [SetUp]
        public void SetUp()
        {
            repositoryOptionsMock = new Mock<IRepositoryOptions>();
            var dbContextOptions = DbContextOptionsTestBuilder.Build(services => services.AddSingleton(repositoryOptionsMock.Object));
            //SetUp code here
            databaseLoggerOptions = new DatabaseLoggerOptions();
            
            databaseLoggerOptions.ConfigureLogStatusTable<LogStatus>(
                logEnabled => logEnabled.Level,
                logEnabled => logEnabled.Active);

            databaseLoggerOptions.ConfigureLogTable<Log>(
                log => log.LogLevelId,
                log => log.EventId,
                log => log.State,
                log => log.Message,
                log => log.FormattedMessage);
            testDbContext = new TestDbContext(dbContextOptions);
            sut = new DatabaseLogger<TestDbContext>(testDbContext,
                databaseLoggerOptions);
        }

        [Test]
        public void IsEnabled()
        {
            testDbContext.Add(new LogStatus { Id = 1, Level = 1, Active = true });
            testDbContext.Add(new LogStatus { Id = 2, Level = 2, Active = true });
            testDbContext.Add(new LogStatus { Id = 3, Level = 3, Active = true });
            testDbContext.Add(new LogStatus { Id = 4, Level = 4, Active = true });
            testDbContext.SaveChanges();
            sut.IsEnabled(LogLevel.Information);
        }

        [Test]
        public void Log()
        {
            //Test code here
            sut.Log(LogLevel.Information, new EventId(1000,"Format Exception"), this,
                new FormatException(), format);
        }

        private string format(DatabaseLoggerTests arg1, Exception arg2)
        {
            return string.Format("{0}: {1}", nameof(arg1), arg2.Message);
        }

        private Mock<IRepositoryOptions> repositoryOptionsMock;
        private DatabaseLoggerOptions databaseLoggerOptions;
        private TestDbContext testDbContext;
        private DatabaseLogger<TestDbContext> sut;
    }
}
