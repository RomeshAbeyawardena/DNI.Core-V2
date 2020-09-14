using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Managers
{
    public interface IDatabaseLogManager<TLog> : IDatabaseLogManager
        where TLog: class
    {
        void Log(TLog logEntry);
        Task LogAsync(TLog logEntry, CancellationToken cancellationToken);
        new TLog Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
        new TLog Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }

    public interface IDatabaseLogManager
    {
        void Log(object logEntry);
        Task LogAsync(object logEntry, CancellationToken cancellationToken);
        object Convert<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
        object Convert<TCategory, TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
}
