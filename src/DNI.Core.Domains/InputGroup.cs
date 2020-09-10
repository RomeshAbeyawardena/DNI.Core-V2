using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
