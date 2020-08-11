using DNI.Core.Contracts;
using System;

namespace DNI.Core.Shared
{
    public class ConvertedValue<TValue> : IConvertValue<TValue>
    {
        public ConvertedValue(TValue value)
        {
            IsSuccessful = true;
            Value = value;
        }

        public ConvertedValue(Exception exception)
        {
            Exception = exception;
        }

        public bool IsSuccessful { get; }
        public TValue Value { get; }
        public Exception Exception { get; }
    }
}
