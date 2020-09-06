using System;

namespace DNI.Core.Contracts
{
    public interface IResponse
    {
        object Result { get; }
    }

    public interface IResponse<T> : IResponse
    {
        Exception Exception { get; }
        bool IsSuccessful { get; }
        new T Result { get; }
    }
}
