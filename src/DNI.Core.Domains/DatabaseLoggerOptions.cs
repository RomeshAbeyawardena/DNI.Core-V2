using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public Type DatabaseLogManagerType { get; private set; }
        public Type DatabaseLogStatusManagerType { get; private set; }

        public Type LogTableType { get; private set; }
        public Type LogStatusTableType { get; private set; }
        public Type LoggingDbContext { get; private set; }

    }
}
