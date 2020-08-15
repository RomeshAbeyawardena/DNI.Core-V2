using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public void Try<TParameter>(
            TParameter parameter,
            Action<TParameter> tryBlock, Action<Exception> catchBlock, Action finallyBlock = null, params Type[] exceptionTypes)
        {
            try
            {
                tryBlock(parameter);
            }
            catch(Exception exception)
            {
                if(!IsExceptionHandled(exception, exceptionTypes))
                {
                    throw;
                }

                catchBlock(exception);
            }
            finally
            {
                finallyBlock();
            }
        }

        public TResult Try<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, TResult> tryBlock, 
            Func<Exception, TResult> catchBlock, 
            Action finallyBlock = null, 
            params Type[] exceptionTypes)
        {
            try
            {
                return tryBlock(parameter);
            }
            catch(Exception exception)
            {
                if(!IsExceptionHandled(exception, exceptionTypes))
                {
                    throw;
                }

                return catchBlock(exception);
            }
            finally
            {
                finallyBlock();
            }
        }

        private bool IsExceptionHandled(
            Exception exception,
            IEnumerable<Type> exceptionTypes)
        {
            var currentExceptionType = exception.GetType();
            return exceptionTypes
                .Any(exceptionType => exceptionType == currentExceptionType);
        }
    }
}
