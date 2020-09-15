using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public abstract class DatabaseLogStatusManagerBase<TLogStatus> :  DatabaseEntityManagerBase<TLogStatus>, IDatabaseLogStatusManager<TLogStatus>
        where TLogStatus : class
    {
        protected DatabaseLogStatusManagerBase(
            IServiceProvider serviceProvider, 
            DatabaseLoggerOptions databaseLoggerOptions, 
            IDapperContext<TLogStatus> dapperContext = null)
            : base(serviceProvider, databaseLoggerOptions, dapperContext)
        {
            
        }

        public abstract bool IsEnabled(LogLevel logLevel);

        public abstract Task<bool> IsEnabledAsync(LogLevel logLevel);

        protected IDapperContext<TLogStatus> LogStatuses => Context;
    }
}
