using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface IInputParserOptions
    {
        IEnumerable<char> InputQuoteGroups { get; set; }
        IEnumerable<char> InputSeparatorGroups { get; set; }
    }
}
