using DNI.Core.Contracts;
using System.Collections.Generic;

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
