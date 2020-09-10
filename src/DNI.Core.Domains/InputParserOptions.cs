using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class InputParserOptions :IInputParserOptions
    {
        public IEnumerable<char> InputQuoteGroups { get; set; }
        public IEnumerable<char> InputSeparatorGroups { get; set; }

        public static IInputParserOptions Default => new InputParserOptions{ 
            InputQuoteGroups = new [] { '"', '\'' },
            InputSeparatorGroups = new [] { ' ' }
        };
    }
}
