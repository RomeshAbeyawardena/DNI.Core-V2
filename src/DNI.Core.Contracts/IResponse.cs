using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
