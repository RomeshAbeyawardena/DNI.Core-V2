using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IValueGenerator
    {
        Func<object, object> GenerateValue { get; }
        bool ExpectsValue { get; }
    }
}
