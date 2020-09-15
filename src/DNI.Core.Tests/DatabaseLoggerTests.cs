using DNI.Core.Contracts;
using DNI.Core.Domains;
using DNI.Core.Extensions;
using DNI.Core.Extensions.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading;
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
        public string Category { get; internal set; }
    }

    public class LogStatus
    {
        [Key]
        public int Id { get; set; }
        public int Level { get; set; }
        public bool Active { get; set; }
    }

    public class TestDatabaseStatusLogManager : DatabaseLogStatusManagerBase<LogStatus>
    {
        public TestDatabaseStatusLogManager(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions) 
            : base(serviceProvider, databaseLoggerOptions)
        {
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            var logLevelId = (int) logLevel;
            return Context.GetValue<TValue>();
        }

        public override Task<bool> IsEnabledAsync(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }
    }

    public class TestDatabaseLogManager : DatabaseLogManagerBase<Log>
    {
        public TestDatabaseLogManager(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions, IDapperContext<Log> dapperContext) 
            : base(serviceProvider, databaseLoggerOptions, dapperContext)
        {
        }

        public override Log Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return new Log()
            {
                Category = typeof(TState).FullName,
                LogLevelId = (int)logLevel,
                EventId = eventId.Name,
                State = JsonSerializer.Serialize(state),
                Message = exception.Message,
                FormattedMessage = formatter(state, exception)
            };
        }

        public override Log Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return new Log()
            {
                Category = typeof(TCategory).FullName,
                LogLevelId = (int)logLevel,
                EventId = eventId.Name,
                State = JsonSerializer.Serialize(state),
                Message = exception.Message,
                FormattedMessage = formatter(state, exception)
            };
        }
    }

    [TestFixture]
    public class DatabaseLoggerTests
    {

        [SetUp]
        public void SetUp()
        {
            repositoryOptionsMock = new Mock<IRepositoryOptions>();
            dapperContextMock = new Mock<IDapperContext<Log>>();
            databaseLoggerOptions = new DatabaseLoggerOptions();
            

            databaseLoggerOptions
                .ConfigureLoggingDbContext<TestDbContext>()
                .ConfigureDatabaseLogManagers<TestDatabaseLogManager, TestDatabaseStatusLogManager>();


            var dbContextOptions = DbContextOptionsTestBuilder.Build(services => services
            .AddSingleton(repositoryOptionsMock.Object)
            .AddTransient(services => testDbContext)
            .AddTransient(services => dapperContextMock.Object)
            .RegisterDatabaseLogging<TestDbContext>(databaseLoggerOptions), out var serviceProvider);
            //SetUp code here
            
            testDbContext = new TestDbContext(dbContextOptions);
            sut = new DatabaseLogger<TestDbContext>(serviceProvider,
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

            Assert.IsTrue(sut.IsEnabled(LogLevel.Information));
        }

        [Test]
        public void Log()
        {
            dapperContextMock.Setup(dapperContext => dapperContext.Insert(It.IsAny<Log>(), false, 3000))
                .Verifiable();

            //Test code here
            sut.Log(LogLevel.Information, new EventId(1000,"Format Exception"), this,
                new FormatException(), format);
            dapperContextMock.Verify(dapperContext => dapperContext.Insert(It.IsAny<Log>(), false, 3000));
        }

        private string format(DatabaseLoggerTests arg1, Exception arg2)
        {
            return string.Format("{0}: {1}", nameof(arg1), arg2.Message);
        }


        private Mock<IDapperContext<Log>> dapperContextMock;
        private Mock<IRepositoryOptions> repositoryOptionsMock;
        private DatabaseLoggerOptions databaseLoggerOptions;
        private TestDbContext testDbContext;
        private DatabaseLogger<TestDbContext> sut;
    }
}
