using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Parser
{
    public interface ICommandParser
    {
        bool TryParse<TSettings>(string input, TSettings settings, out ICommand command);
    }
}
