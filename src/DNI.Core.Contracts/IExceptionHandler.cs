using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IExceptionHandler
    {
        Task<TResult> TryAsync<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, Task<TResult>> tryBlock, 
            Func<Exception, Task<TResult>> catchBlock, 
            Action<IDefinition<Type>> exceptionTypes,
            Action finallyBlock = null);

        Task<TResult> TryAsync<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, Task<TResult>> tryBlock, 
            Func<Exception, Task<TResult>> catchBlock, 
            Action finallyBlock = null, 
            params Type[] exceptionTypes);

        void Try(Action tryBlock,
            Action<Exception> catchBlock,
            Action finallyBlock = null,
            params Type[] exceptionTypes);

        void Try(Action tryBlock,
            Action<Exception> catchBlock,
            Action<IDefinition<Type>> exceptionTypes,
            Action finallyBlock = null);

        TResult Try<TParameter, TResult>(
            TParameter parameter,
            Func<TParameter, TResult> tryBlock, 
            Func<Exception, TResult> catchBlock, 
            Action finallyBlock = null,
            params Type[] exceptionTypes);

        void Try<TParameter>(
            TParameter parameter,
            Action<TParameter> tryBlock, 
            Action<Exception> catchBlock, 
            Action finallyBlock = null,
            params Type[] exceptionTypes);

        TResult Try<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, TResult> tryBlock, 
            Func<Exception, TResult> catchBlock, 
            Action<IDefinition<Type>> exceptionTypes,
            Action finallyBlock = null);
    }
}
