using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Builders
{
    public interface IDictionaryBuilder<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value);
        IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> keyValuePair);
    }
}
