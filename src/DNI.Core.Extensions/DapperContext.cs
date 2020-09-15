using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions
{
    public class DapperContext<T> : IDisposable
        where T: class
    {
        public DapperContext(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Insert(T value, bool useTransaction, int timeout = 3000)
        {
            if(dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            if (useTransaction && transaction == null)
            {
                transaction = dbConnection.BeginTransaction();
            }

            dbConnection.Insert(value, transaction, timeout);
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

        private readonly IDbConnection dbConnection;
        private bool disposedValue;
        private IDbTransaction transaction;
    }
}
