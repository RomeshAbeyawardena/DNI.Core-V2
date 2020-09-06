using System.Collections.Generic;

namespace DNI.Core.Contracts.Builders
{
    public interface IDictionaryBuilder<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IDictionary<TKey, TValue> Dictionary { get; }
        IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value);
        IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> keyValuePair);
    }
}
