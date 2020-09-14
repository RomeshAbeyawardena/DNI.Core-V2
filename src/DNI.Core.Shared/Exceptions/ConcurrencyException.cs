using DNI.Core.Shared.Enumerations;
using System;

namespace DNI.Core.Shared.Exceptions
{
    [Serializable]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(ConcurrentAction action, string message) : base(message) { Action = action; }
        public ConcurrencyException(ConcurrentAction action,string message, Exception inner) : base(message, inner) { Action = action; }

        public ConcurrentAction Action { get; }

        protected ConcurrencyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
