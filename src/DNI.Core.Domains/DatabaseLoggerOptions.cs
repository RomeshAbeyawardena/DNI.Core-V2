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
        public void ConfigureLoggingDbContext<TDbContext>()
        {
            LoggingDbContext = typeof(TDbContext);
        }

        public void ConfigureLogTable<TLog>()
        {
            LogTableType = typeof(TLog);
        }

        public void ConfigureLogStatusTable<TLogStatus>()
        {
            LogStatusTableType = typeof(TLogStatus);
        }

        public void ConfigureDatabaseLogManagers<TDatabaseLogManager>()
            where TDatabaseLogManager : IDatabaseLogManager
        {
            DatabaseLogManagerType = typeof(TDatabaseLogManager);
        }

        public void ConfigureDatabaseLogManagers<TDatabaseLogManager, TDatabaseLogStatusManager>()
            where TDatabaseLogManager : IDatabaseLogManager
            where TDatabaseLogStatusManager : IDatabaseLogStatusManager
        {
            ConfigureDatabaseLogManagers<TDatabaseLogManager>();
            DatabaseLogStatusManagerType = typeof(IDatabaseLogStatusManager);
        }

        public Type DatabaseLogManagerType { get; private set; }
        public Type DatabaseLogStatusManagerType { get; private set; }

        public Type LogTableType { get; private set; }
        public Type LogStatusTableType { get; private set; }
        public Type LoggingDbContext { get; private set; }

    }
}
