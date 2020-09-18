using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DNI.Core.Domains
{
    public class DatabaseLoggerOptions
    {
        public DatabaseLoggerOptions ConfigureLoggingDbContext<TDbContext>()
        {
            LoggingDbContext = typeof(TDbContext);
            return this;
        }

        public DatabaseLoggerOptions ConfigureLogTable<TLog>()
        {
            return ConfigureLogTable(typeof(TLog));
        }

        public DatabaseLoggerOptions ConfigureLogStatusTable(Type logStatus)
        {
            LogStatusTableType = logStatus;
            return this;
        }

        public DatabaseLoggerOptions ConfigureLogTable(Type log)
        {
            LogTableType = log;
            return this;
        }

        public DatabaseLoggerOptions ConfigureLogStatusTable<TLogStatus>()
        {
            return ConfigureLogStatusTable(typeof(TLogStatus));
        }


        public DatabaseLoggerOptions ConfigureDatabaseLogManagers<TDatabaseLogManager>()
            where TDatabaseLogManager : IDatabaseLogManager
        {
            DatabaseLogManagerType = typeof(TDatabaseLogManager);

            if(LogTableType == null)
            {
                var interfaceTypes = DatabaseLogManagerType.GetInterfaces();
                var logTableType = interfaceTypes.FirstOrDefault()
                    .GenericTypeArguments
                    .FirstOrDefault();

                ConfigureLogTable(logTableType);
            }


            return this;
        }

        public DatabaseLoggerOptions ConfigureDatabaseLogManagers<TDatabaseLogManager, TDatabaseLogStatusManager>()
            where TDatabaseLogManager : IDatabaseLogManager
            where TDatabaseLogStatusManager : IDatabaseLogStatusManager
        {
            ConfigureDatabaseLogManagers<TDatabaseLogManager>();
            DatabaseLogStatusManagerType = typeof(TDatabaseLogStatusManager);

            if(LogStatusTableType == null)
            {
                var interfaceTypes = DatabaseLogStatusManagerType.GetInterfaces();
                var logTableStatusType = interfaceTypes.FirstOrDefault()
                    .GenericTypeArguments
                    .FirstOrDefault();

                if(logTableStatusType != null)
                { 
                    ConfigureLogStatusTable(logTableStatusType);
                }
            }
            return this;
        }



        public DatabaseLoggerOptions ConfigureLogStatusManagerFromServiceProvider<TConfiguration>(
            Func<IServiceProvider, TConfiguration> configuration)
            where TConfiguration : ILogStatusConfiguration
        {
            LogStatusManagerConfiguration = serviceProvider => configuration(serviceProvider);
            ConfigureLogStatusManager<TConfiguration>(false);
            return this;
        }

        public DatabaseLoggerOptions ConfigureLogStatusManager<TConfiguration>(bool registerDefaultService = true)
            where TConfiguration : ILogStatusConfiguration
        {
            ConfigurationType = typeof(TConfiguration);
            RegisterDefaultLogStatusService = registerDefaultService;
            return this;
        }

        public DatabaseLoggerOptions ConfigureLogStatusManager<TConfiguration>(
            Func<IConfiguration, TConfiguration> configure)
            where TConfiguration : ILogStatusConfiguration
        {
            return ConfigureLogStatusManagerFromServiceProvider(serviceProvider =>  { 
                var configuration = serviceProvider.GetRequiredService<IConfiguration>(); 
                return configure(configuration); 
            });
        }

        public bool RegisterDefaultLogStatusService { get; private set; }
        public Func<IServiceProvider, ILogStatusConfiguration> LogStatusManagerConfiguration;
        public Type DatabaseLogManagerType { get; private set; }
        public Type DatabaseLogStatusManagerType { get; private set; }
        public Type ConfigurationType { get; private set; }
        public Type LogTableType { get; private set; }
        public Type LogStatusTableType { get; private set; }
        public Type LoggingDbContext { get; private set; }

    }
}
