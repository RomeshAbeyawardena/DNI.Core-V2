using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Parser
{
    public interface IInputParser
    {
        IInputGroup Parse(string input, IInputParserOptions inputParserOptions = null);
        IInputParserOptions Options { get; }
    }
}
