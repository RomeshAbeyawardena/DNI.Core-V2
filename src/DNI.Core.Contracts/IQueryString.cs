using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IQueryString : IReadOnlyDictionary<string, string>
    {
        IQueryString Append(string key, string value);
        IQueryString Append(IEnumerable<KeyValuePair<string, string>> keyValuePairs);
    }
}
