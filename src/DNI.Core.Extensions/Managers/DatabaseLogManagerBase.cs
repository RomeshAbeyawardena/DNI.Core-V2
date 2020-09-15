using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public abstract class DatabaseLogManagerBase<TLog> : IDatabaseLogManager<TLog>, 
        IDatabaseLogManager
        where TLog: class
    {
        protected DatabaseLogManagerBase(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            var dbContext = serviceProvider.GetService(databaseLoggerOptions.LoggingDbContext) as DbContext;
            LogRepository = new DapperContext<TLog>(dbContext.Database.GetDbConnection()); 
        }

        public abstract TLog Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        public abstract TLog Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        public virtual void Log(TLog logEntry)
        {
            LogRepository.Insert(logEntry, false);
        }

        public virtual Task LogAsync(TLog logEntry, CancellationToken cancellationToken)
        {
            LogRepository.Insert(logEntry, false);
            return Task.CompletedTask;
        }

        void IDatabaseLogManager.Log(object logEntry)
        {
            Log(logEntry as TLog);
        }

        Task IDatabaseLogManager.LogAsync(object logEntry, CancellationToken cancellationToken)
        {
            return LogAsync(logEntry as TLog, cancellationToken);
        }

        object IDatabaseLogManager.Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return Convert(logLevel, eventId, state, exception, formatter);
        }

        object IDatabaseLogManager.Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return Convert<TCategory, TState>(logLevel, eventId, state, exception, formatter);
        }

        protected DapperContext<TLog> LogRepository { get; }
    }
}
