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

            var logItem = DatabaseLogManager
                .Convert<TCategoryName, TState>(logLevel, eventId, state, exception, formatter);

            if (IsEnabled(logLevel))
            {
                DatabaseLogManager.Log(logItem);
            }
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
            if (databaseLogStatusManager == null)
            {
                return false;
            }

            return databaseLogStatusManager.IsEnabled(logLevel);
        }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            PrepareServices(rootServiceProvider);
            if (DatabaseLogManager == null || !IsEnabled(logLevel))
            {
                return;
            }

            var convertedLogItem = DatabaseLogManager.Convert(logLevel, eventId, state, exception, formatter);
            DatabaseLogManager.Log(convertedLogItem);
        }


        internal void PrepareServices(IServiceProvider serviceProvider)
        {
            if (DatabaseLogManager == null)
            {
                scopedServiceProvider = serviceProvider.CreateScope();
                var databaseLogManagerType = typeof(IDatabaseLogManager<>);

                var genericDatabaseLogManagerType = databaseLogManagerType.MakeGenericType(databaseLoggerOptions.LogTableType);

                DatabaseLogManager = ServiceProvider.GetService(genericDatabaseLogManagerType) as IDatabaseLogManager;
            }

            if (databaseLogStatusManager == null)
            {
                var databaseLogStatusManagerType = typeof(IDatabaseLogStatusManager<>);
                if (databaseLoggerOptions.LogStatusTableType != null)
                {
                    var genericDatabaseLogStatusManagerType =
                        databaseLogStatusManagerType.MakeGenericType(databaseLoggerOptions.LogStatusTableType);

                    databaseLogStatusManager = ServiceProvider.GetService(genericDatabaseLogStatusManagerType) as IDatabaseLogStatusManager;
                }
                else if (databaseLoggerOptions.ConfigurationType != null)
                {
                    var genericDatabaseLogStatusManagerType =
                        databaseLogStatusManagerType.MakeGenericType(databaseLoggerOptions.ConfigurationType);

                    databaseLogStatusManager = ServiceProvider.GetService(genericDatabaseLogStatusManagerType) as IDatabaseLogStatusManager;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool gc)
        {
            scopedServiceProvider?.Dispose();
        }

        protected IDatabaseLogManager DatabaseLogManager { get; private set; }
        protected IServiceProvider ServiceProvider => scopedServiceProvider.ServiceProvider;
        private IDatabaseLogStatusManager databaseLogStatusManager;
        private IServiceScope scopedServiceProvider;
        internal readonly IServiceProvider rootServiceProvider;
        private readonly DatabaseLoggerOptions databaseLoggerOptions;
    }
}
