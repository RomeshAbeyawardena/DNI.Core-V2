using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IDapperContext : IDisposable
    {
        IEnumerable<T> Query<T>(string sql, T parameters);
        T Get<T>(string sql, T parameters);
        long Insert(object value, bool useTransaction, int timeout = 3000);
        int Execute(string sql, object parameters, bool useTransaction, int timeout = 3000);
        void Rollback();
        void Commit();
    }
     
    public interface IDapperContext<T> : IDapperContext
    {
        IEnumerable<T> Query(string sql, T parameters);
        T Get(string sql, T parameters);
        long Insert(T value, bool useTransaction, int timeout = 3000);
        int Execute(string sql, T parameters, bool useTransaction, int timeout = 3000);
    }
}
