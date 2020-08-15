using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Builders
{
    public class DictionaryBuilder<TKey, TValue> : IDictionaryBuilder<TKey, TValue>
    {
        public static IDictionaryBuilder<TKey, TValue> Create()
        {
            return new DictionaryBuilder<TKey, TValue>();
        }

        protected DictionaryBuilder()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => dictionary[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => dictionary.Count;

        public IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            return this;
        }

        IDictionaryBuilder<TKey, TValue> IDictionaryBuilder<TKey, TValue>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private Dictionary<TKey, TValue> dictionary;
    }
}
