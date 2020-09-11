using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface IInputGroup
    {
        IEnumerable<string> ParsedValues { get; }
    }
}
