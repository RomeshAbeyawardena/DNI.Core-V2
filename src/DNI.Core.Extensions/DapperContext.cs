using Dapper;
using Dapper.Contrib.Extensions;
using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions
{
    public class DapperContext<T> : IDapperContext<T>
        where T: class
    {
        public DapperContext(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public long Insert(T value, bool useTransaction, int timeout = 3000)
        {
            EnsureConnectionIsReady();
            PrepareTransaction(useTransaction);
            return dbConnection.Insert(value, transaction, timeout);
        }

        public int Execute(string sql, T parameters, bool useTransaction, int timeout = 3000)
        {
            EnsureConnectionIsReady();
            PrepareTransaction(useTransaction);
            var commandDefinition = new CommandDefinition(sql, parameters, transaction, timeout);
            
            return dbConnection.Execute(commandDefinition);
        }

        public void Rollback()
        {
            transaction.Rollback();
            ResetTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
            ResetTransaction();
        }

        protected void EnsureConnectionIsReady()
        {
            if(!TryPrepareConnection())
            {
                throw new DataException("Database connection not ready");
            }

        }

        protected bool TryPrepareConnection()
        {
            if(dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            return dbConnection.State == ConnectionState.Open;
        }

        protected void PrepareTransaction(bool useTransaction)
        {
            if (useTransaction && transaction == null)
            {
                transaction = dbConnection.BeginTransaction();
            }

        }

        protected void ResetTransaction()
        {
            transaction.Dispose();
            transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    transaction.Dispose();
                    dbConnection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        long IDapperContext.Insert(object value, bool useTransaction, int timeout)
        {
            return Insert(value as T, useTransaction, timeout);
        }

        int IDapperContext.Execute(string sql, object parameters, bool useTransaction, int timeout)
        {
            return Execute(sql, parameters as T, useTransaction, timeout);
        }

        private readonly IDbConnection dbConnection;
        private bool disposedValue;
        private IDbTransaction transaction;
    }
}
