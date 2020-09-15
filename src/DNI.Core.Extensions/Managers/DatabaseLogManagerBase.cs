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
    public abstract class DatabaseLogManagerBase<TLog> : DatabaseEntityManagerBase<TLog>, IDatabaseLogManager<TLog>, 
        IDatabaseLogManager
        where TLog: class
    {
        protected DatabaseLogManagerBase(
            IServiceProvider serviceProvider, 
            DatabaseLoggerOptions databaseLoggerOptions, 
            IDapperContext<TLog> dapperContext = null)
            : base(serviceProvider, databaseLoggerOptions, dapperContext)
        {
            
        }

        public abstract TLog Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        public abstract TLog Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        public virtual void Log(TLog logEntry)
        {
            Context.Insert(logEntry, false);
        }

        public virtual Task LogAsync(TLog logEntry, CancellationToken cancellationToken)
        {
            Context.Insert(logEntry, false);
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
    }
}
