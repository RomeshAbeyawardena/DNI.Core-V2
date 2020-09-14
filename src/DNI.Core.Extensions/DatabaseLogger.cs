using DNI.Core.Domains;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            PrepareServices(rootServiceProvider);
            DatabaseLogManager.Log(DatabaseLogManager
                .Convert<TCategoryName, TState>(logLevel, eventId, state, exception, formatter));
        }
    }

    public class DatabaseLogger : ILogger, IDisposable
    {
        public DatabaseLogger(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            this.rootServiceProvider = serviceProvider;
            this.databaseLoggerOptions = databaseLoggerOptions;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            PrepareServices(rootServiceProvider);
            if(databaseLogStatusManager == null)
            {
                return false;
            }

            return databaseLogStatusManager.IsEnabled(logLevel);
        }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            PrepareServices(rootServiceProvider);
            if(DatabaseLogManager == null)
            { 
                return;
            }

            DatabaseLogManager.Log(
                DatabaseLogManager.Convert(logLevel, eventId, state, exception, formatter));
        }

        
        internal void PrepareServices(IServiceProvider serviceProvider)
        {
            scopedServiceProvider = serviceProvider.CreateScope();
            var databaseLogManagerType = typeof(IDatabaseLogManager<>);
            var databaseLogStatusManagerType = typeof(IDatabaseLogStatusManager<>);

            var genericDatabaseLogManagerType = databaseLogManagerType.MakeGenericType(databaseLoggerOptions.LogTableType);

            DatabaseLogManager =  ServiceProvider.GetService(genericDatabaseLogManagerType) as IDatabaseLogManager;
            
            if(databaseLoggerOptions.LogStatusTableType != null)
            { 
                var genericDatabaseLogStatusManagerType = 
                    databaseLogStatusManagerType.MakeGenericType(databaseLoggerOptions.LogStatusTableType);

                databaseLogStatusManager = ServiceProvider.GetService(genericDatabaseLogStatusManagerType) as IDatabaseLogStatusManager;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool gc)
        {
            scopedServiceProvider.Dispose();
        }

        protected IDatabaseLogManager DatabaseLogManager { get; private set; }
        protected IServiceProvider ServiceProvider => scopedServiceProvider.ServiceProvider;
        private IDatabaseLogStatusManager databaseLogStatusManager;
        private IServiceScope scopedServiceProvider;
        internal readonly IServiceProvider rootServiceProvider;
        private readonly DatabaseLoggerOptions databaseLoggerOptions;
    }
}
