using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Parser
{
    public interface IInputParserOptions
    {
        IEnumerable<char> InputQuoteGroups { get; set; }
        IEnumerable<char> InputSeparatorGroups { get; set; }
    }
}
