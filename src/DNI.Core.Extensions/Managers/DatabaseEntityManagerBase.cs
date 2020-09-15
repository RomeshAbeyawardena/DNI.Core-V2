using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public class DatabaseEntityManagerBase<TEntity> : IDatabaseEntityManager<TEntity>
        where TEntity : class
    {
        protected DatabaseEntityManagerBase(IServiceProvider serviceProvider, 
            DatabaseLoggerOptions databaseLoggerOptions, 
            IDapperContext<TEntity> dapperContext = null)
        {
            var dbContext = serviceProvider.GetService(databaseLoggerOptions.LoggingDbContext) as DbContext;
            Context = dapperContext == null 
                ? new DapperContext<TEntity>(dbContext.Database.GetDbConnection())
                : dapperContext; 
        }

        public IDapperContext<TEntity> Context { get; }

        IDapperContext IDatabaseEntityManager.Context => Context;
    }
}
