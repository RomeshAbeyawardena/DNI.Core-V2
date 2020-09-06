using DNI.Core.Contracts.Builders;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DNI.Core.Services.Builders
{
    public static class DictionaryBuilder 
    {
        public static IDictionaryBuilder<TKey, TValue> Create<TKey, TValue>(Action<IDictionaryBuilder<TKey, TValue>> builderAction = null)
        {
            var dictionaryBuilder = DictionaryBuilder<TKey, TValue>.Create();
            builderAction?.Invoke(dictionaryBuilder);
            return dictionaryBuilder;
        }
    }

    [IgnoreScanning]
    public class DictionaryBuilder<TKey, TValue> : IDictionaryBuilder<TKey, TValue>
    {
        public static IDictionaryBuilder<TKey, TValue> Create()
        {
            return new DictionaryBuilder<TKey, TValue>();
        }

        protected DictionaryBuilder()
        {
            Dictionary = new Dictionary<TKey, TValue>();
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => Dictionary[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => Dictionary.Count;

        public IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
            return this;
        }

        IDictionaryBuilder<TKey, TValue> IDictionaryBuilder<TKey, TValue>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        public IDictionary<TKey, TValue> Dictionary { get; }
    }
}
