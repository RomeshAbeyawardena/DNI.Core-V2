using DNI.Core.Contracts;
using System.Collections.Generic;

namespace DNI.Core.Domains
{
    public class InputGroup : IInputGroup
    {
        public InputGroup(params string[] parsedValues)
        {
            ParsedValues = parsedValues;
        }

        public IEnumerable<string> ParsedValues { get; }
    }
}
