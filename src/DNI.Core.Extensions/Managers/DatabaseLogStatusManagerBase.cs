using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public abstract class DatabaseLogStatusManagerBase<TLogStatus> : IDatabaseLogStatusManager<TLogStatus>
        where TLogStatus : class
    {
        protected DatabaseLogStatusManagerBase(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            DbContext = serviceProvider.GetService(databaseLoggerOptions.LoggingDbContext) as DbContext;
            LogStatuses = DbContext.Set<TLogStatus>();
        }

        public abstract bool IsEnabled(LogLevel logLevel);

        public abstract Task<bool> IsEnabledAsync(LogLevel logLevel);

        protected DbContext DbContext { get; }
        protected DbSet<TLogStatus> LogStatuses { get; }
    }
}
