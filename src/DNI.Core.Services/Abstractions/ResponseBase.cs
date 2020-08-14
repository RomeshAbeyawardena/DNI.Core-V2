using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ResponseBase<T> : IResponse<T>
    {
        protected ResponseBase(T result)
        {
            Result = result;
            IsSuccessful = result != null;
        }

        protected ResponseBase(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }

        public bool IsSuccessful { get; }

        public T Result { get; }

        object IResponse.Result => Result;
    }
}
