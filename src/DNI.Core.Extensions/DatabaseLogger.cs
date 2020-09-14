using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DNI.Core.Services.Extensions;
using DNI.Core.Contracts.Managers;

namespace DNI.Core.Extensions
{
    public sealed class DatabaseLogger<TCategoryName> : DatabaseLogger, ILogger<TCategoryName>
    {
        public DatabaseLogger(IServiceProvider dbContext, DatabaseLoggerOptions databaseLoggerOptions) 
            : base(dbContext, databaseLoggerOptions)
        {
        }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            DatabaseLogManager.Log(DatabaseLogManager
                .Convert<TCategoryName, TState>(logLevel, eventId, state, exception, formatter));
        }
    }

    public class DatabaseLogger : ILogger
    {
        public DatabaseLogger(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            var databaseLogManagerType = typeof(IDatabaseLogManager<>);
            var databaseLogStatusManagerType = typeof(IDatabaseLogStatusManager<>);

            var genericDatabaseLogManagerType = databaseLogManagerType.MakeGenericType(databaseLoggerOptions.LogTableType);

            var genericDatabaseLogStatusManagerType = 
                databaseLogStatusManagerType.MakeGenericType(databaseLoggerOptions.LogStatusTableType);

            var service1 =  serviceProvider.GetService(genericDatabaseLogManagerType);
            var service2 = serviceProvider.GetService(genericDatabaseLogStatusManagerType);

            DatabaseLogManager = service1 as IDatabaseLogManager;
            databaseLogStatusManager = service2 as IDatabaseLogStatusManager;
            
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if(databaseLogStatusManager == null)
            {
                return true;
            }

            return databaseLogStatusManager.IsEnabled(logLevel);
        }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(DatabaseLogManager == null)
            { 
                return;
            }

            DatabaseLogManager.Log(
                DatabaseLogManager.Convert(logLevel, eventId, state, exception, formatter));
        }


        protected IDatabaseLogManager DatabaseLogManager { get; }

        private readonly IDatabaseLogStatusManager databaseLogStatusManager;
    }
}
