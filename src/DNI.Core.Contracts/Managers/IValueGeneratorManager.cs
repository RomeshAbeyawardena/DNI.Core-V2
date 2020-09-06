using System;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Managers
{
    public interface IValueGeneratorManager: IReadOnlyDictionary<string, Type>
    {
        void AppendValueGenerators(IEnumerable<KeyValuePair<string, Type>> valueKeyPairs);
    }
}
