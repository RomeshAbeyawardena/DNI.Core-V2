using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public abstract class DatabaseLogManagerBase<TLog> : IDatabaseLogManager<TLog>
        , IDatabaseLogManager
        where TLog: class
    {
        protected DatabaseLogManagerBase(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            DbContext = serviceProvider.GetService(databaseLoggerOptions.LoggingDbContext) as DbContext;
            Logs = DbContext.Set<TLog>();
        }

        public abstract TLog Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        public abstract TLog Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        public abstract void Log(TLog logEntry);
        public abstract Task LogAsync(TLog logEntry, CancellationToken cancellationToken);

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

        protected DbContext DbContext { get; }
        protected DbSet<TLog> Logs { get; }
    }
}
