using DNI.Core.Contracts.Managers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public abstract class DatabaseLogStatusManagerBase<TLogStatus> : IDatabaseLogStatusManager<TLogStatus>
    {
        protected DatabaseLogManagerBase(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            DbContext = serviceProvider.GetService(databaseLoggerOptions.LoggingDbContext) as DbContext;
            Logs = DbContext.Set<TLog>();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEnabledAsync(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }
    }
}
