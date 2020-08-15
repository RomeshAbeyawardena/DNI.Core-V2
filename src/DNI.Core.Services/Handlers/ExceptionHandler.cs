using DNI.Core.Contracts;
using DNI.Core.Services.Definitions;
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
                finallyBlock?.Invoke();
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
                finallyBlock?.Invoke();
            }
        }

        public void Try(Action tryBlock, Action<Exception> catchBlock, Action finallyBlock = null, params Type[] exceptionTypes)
        {
            try
            {
               tryBlock();
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
                finallyBlock?.Invoke();
            }
        }

        public void Try(Action tryBlock, Action<Exception> catchBlock, Action<ITypeDefinition> exceptionTypes, Action finallyBlock = null)
        {
            var typeDefinition = new TypeDefinition();

            exceptionTypes(typeDefinition);
            Try(tryBlock, catchBlock, finallyBlock, typeDefinition.Types.ToArray());
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
