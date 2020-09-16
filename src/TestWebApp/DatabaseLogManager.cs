using DNI.Core.Contracts;
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
        public DatabaseLogManager(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions) 
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
            return new Log
            {
                LogLevelId = (int)logLevel,
                EventId = eventId.Id,
                EventName = eventId.Name,
                CategoryName = typeof(TState).FullName,
                Message = formatter(state, exception)
            };
        }

        public override Log Convert<TCategory, TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            return new Log
            {
                LogLevelId = (int)logLevel,
                EventId = eventId.Id,
                EventName = eventId.Name,
                CategoryName = typeof(TCategory).FullName,
                Message = formatter(state, exception)
            };
        }

        public override void Log(Log logEntry)
        {
            //base.Context.Execute("EXECUTE [Utility].[usp_Log] @logLevelId" +
            //                                                ",@eventId" +
            //                                                ",@eventName" +
            //                                                ",@categoryName" +
            //                                                ",@message", logEntry, false);
            base.Log(logEntry);
        }
    }
}
