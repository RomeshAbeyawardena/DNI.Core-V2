using DNI.Core.Contracts;
using DNI.Core.Services.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<TResult> TryAsync<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, Task<TResult>> tryBlock, 
            Func<Exception, Task<TResult>> catchBlock, 
            Action finallyBlock = null, 
            params Type[] exceptionTypes)
        {
            try
            {
                return await tryBlock(parameter);
            }
            catch(Exception exception)
            {
                if(!IsExceptionHandled(exception, exceptionTypes))
                {
                    throw;
                }

                return await catchBlock(exception);
            }
            finally
            {
                finallyBlock?.Invoke();
            }
        }

        public async Task<TResult> TryAsync<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, Task<TResult>> tryBlock, 
            Func<Exception, Task<TResult>> catchBlock, 
            Action<IDefinition<Type>> exceptionTypes,
            Action finallyBlock = null)
        {
            var typeDefinitions = TypeDefinition.Default;
            exceptionTypes.Invoke(typeDefinitions);
            return await TryAsync<TParameter, TResult>(parameter, tryBlock, catchBlock, finallyBlock, typeDefinitions.ToArray());
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

        public void Try(Action tryBlock, Action<Exception> catchBlock, Action<IDefinition<Type>> exceptionTypes, Action finallyBlock = null)
        {
            var typeDefinition = TypeDefinition.Default;
            exceptionTypes(typeDefinition);

            Try(tryBlock, catchBlock, finallyBlock, typeDefinition.Definitions.ToArray());
        }

        
        public TResult Try<TParameter, TResult>(
            TParameter parameter, 
            Func<TParameter, TResult> tryBlock, 
            Func<Exception, TResult> catchBlock, 
            Action<IDefinition<Type>> exceptionTypes,
            Action finallyBlock = null)
        {
            var typeDefinition = TypeDefinition.Default;
            exceptionTypes(typeDefinition);

            return Try(parameter, tryBlock, catchBlock, finallyBlock, typeDefinition.Definitions.ToArray());
        }

        private bool IsExceptionHandled(
            Exception exception,
            IEnumerable<Type> exceptionTypes)
        {
            var currentExceptionType = exception.GetType();
            return exceptionTypes
                .Any(exceptionType => exceptionType == currentExceptionType);
        }

        public async Task TryAsync<TParameter>(TParameter parameter, Func<TParameter, Task> tryBlock, Func<Exception, Task> catchBlock, Action<IDefinition<Type>> exceptionTypes, Action finallyBlock = null)
        {
            var typeDefinitions = TypeDefinition.Default;

            exceptionTypes(typeDefinitions);

            try
            {
                await tryBlock(parameter);
            }
            catch (Exception exception)
            {
                if(!IsExceptionHandled(exception, typeDefinitions.Definitions))
                {
                    throw;
                }

                await catchBlock(exception);
            }
            finally
            {
                finallyBlock?.Invoke();
            }
        }
    }
}
