using System;

namespace DNI.Core.Contracts
{
    public interface IAttempt
    {
        Exception Exception { get; }
        bool Successful { get; }
        object Result { get; }
    }

    public interface IAttempt<T> : IAttempt
        where T: class
    {
        new T Result { get; }
    }
}
