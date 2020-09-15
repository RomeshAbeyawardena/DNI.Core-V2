using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IDapperContext : IDisposable
    {
        long Insert(object value, bool useTransaction, int timeout = 3000);
        int Execute(string sql, object parameters, bool useTransaction, int timeout = 3000);
        void Rollback();
        void Commit();
    }
     
    public interface IDapperContext<T> : IDapperContext
    {
        long Insert(T value, bool useTransaction, int timeout = 3000);
        int Execute(string sql, T parameters, bool useTransaction, int timeout = 3000);
    }
}
