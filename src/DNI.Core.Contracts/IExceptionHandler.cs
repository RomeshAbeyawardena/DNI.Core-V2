using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IExceptionHandler
    {
        void Try(Action tryBlock,
            Action<Exception> catchBlock,
            Action finallyBlock = null,
            params Type[] exceptionTypes);

        void Try(Action tryBlock,
            Action<Exception> catchBlock,
            Action<ITypeDefinition> exceptionTypes,
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
    }
}
