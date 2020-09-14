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
            base.Log(logLevel, eventId, state, exception, formatter);
        }
    }

    public class DatabaseLogger : ILogger
    {
        public DatabaseLogger(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            var databaseLogManagerType = typeof(IDatabaseLogManager<>);

            var genericDatabaseLogManagerType = databaseLogManagerType.MakeGenericType(databaseLoggerOptions.LogTableType);
            databaseLogManager =  (IDatabaseLogManager) serviceProvider.GetService(genericDatabaseLogManagerType);

            
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            
            return true;
        }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            databaseLogManager.Log(
                databaseLogManager.Convert(logLevel, eventId, state, exception, formatter));
        }

        private void SetProperty(Type type, object instance, 
            string propertyName, object value)
        {
            var memberInfo = type.GetProperty(propertyName);

            memberInfo.SetValue(instance, value);
        }
        
        private readonly IDatabaseLogManager databaseLogManager;
        private readonly DatabaseLoggerOptions databaseLoggerOptions;
    }
}
