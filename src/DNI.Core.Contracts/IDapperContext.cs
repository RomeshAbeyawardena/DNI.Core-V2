using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IDapperContext : IDisposable
    {
        IEnumerable<object> Query(string sql, object parameters);
        object Get(string sql, object parameters);
        TResult Get<TParameter, TResult>(string sql, TParameter parameter);
        long Insert(object value, bool useTransaction, int timeout = 3000);
        int Execute(string sql, object parameters, bool useTransaction, int timeout = 3000);
        void Rollback();
        void Commit();
    }
     
    public interface IDapperContext<T> : IDapperContext
    {
        IEnumerable<T> Query(string sql, T parameters);
        IEnumerable<T> Query<TParameter>(string sql, TParameter parameters);
        T Get(string sql, T parameters);
        T Get<TParameter>(string sql, TParameter parameters);
        long Insert(T value, bool useTransaction, int timeout = 3000);
        int Execute(string sql, T parameters, bool useTransaction, int timeout = 3000);
    }
}
