using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public abstract class DatabaseEntityManagerBase<TEntity> : IDatabaseEntityManager<TEntity>
        where TEntity : class
    {
        protected DatabaseEntityManagerBase(IServiceProvider serviceProvider,
            DatabaseLoggerOptions databaseLoggerOptions,
            IDapperContext<TEntity> dapperContext = null)
        {
            if (dapperContext == null)
            {
                var dbContext = GetServiceScope(serviceProvider)
                        .ServiceProvider
                        .GetService(databaseLoggerOptions.LoggingDbContext) as DbContext;

                Context = new DapperContext<TEntity>(dbContext.Database.GetDbConnection());
                return;
            }

            Context = dapperContext;
        }

        public IDapperContext<TEntity> Context { get; }

        IDapperContext IDatabaseEntityManager.Context => Context;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServiceScope?.Dispose();
            }
            Debug.WriteLine("Disposed", nameof(DatabaseEntityManagerBase<TEntity>));
            GC.SuppressFinalize(this);
        }

        private IServiceScope GetServiceScope(IServiceProvider serviceProvider) => ServiceScope ??= serviceProvider.CreateScope();

        private IServiceScope ServiceScope { get; set; }
    }
}
