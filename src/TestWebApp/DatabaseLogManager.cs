using DNI.Core.Domains;
using DNI.Core.Extensions.Managers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestWebApp
{
    public class DatabaseLogManager : DatabaseLogManagerBase<Log>
    {
        public DatabaseLogManager(
            IServiceProvider serviceProvider, 
            DatabaseLoggerOptions databaseLoggerOptions) 
            : base(serviceProvider, databaseLoggerOptions)
        {
        }

        public override Log Convert<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public override Log Convert<TCategory, TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public override void Log(Log logEntry)
        {
            throw new NotImplementedException();
        }

        public override Task LogAsync(Log logEntry, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
