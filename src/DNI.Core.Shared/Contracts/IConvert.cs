using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IConvert
    {
        IConvertValue<TValue> TryConvert<TValue>(object value);
    }
}
