using DNI.Core.Contracts;
using DNI.Core.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class QueryString : IQueryString
    {
        public static IQueryString Create()
        {
            return new QueryString();
        }

        public IQueryString Append(string key, string value)
        {
            queryStringDictionary.Add(key, value);
            return this;
        }

        public IQueryString Append(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            keyValuePairs
                .ForEach(keyValuePair => queryStringDictionary.Add(keyValuePair));
            return this;
        }

        public override string ToString()
        {
            return string.Join("&", queryStringDictionary.Select(keyValuePair => string.IsNullOrEmpty(keyValuePair.Value) 
                ? keyValuePair.Key 
                : string.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value)));
        }

        bool IReadOnlyDictionary<string, string>.ContainsKey(string key)
        {
            return queryStringDictionary.ContainsKey(key);
        }

        bool IReadOnlyDictionary<string, string>.TryGetValue(string key, out string value)
        {
            return queryStringDictionary.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return queryStringDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return queryStringDictionary.GetEnumerator();
        }

        private QueryString()
        {
            queryStringDictionary = new Dictionary<string, string>();
        }

        private readonly IDictionary<string, string> queryStringDictionary;

        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => queryStringDictionary.Keys;

        IEnumerable<string> IReadOnlyDictionary<string, string>.Values => queryStringDictionary.Values;

        int IReadOnlyCollection<KeyValuePair<string, string>>.Count => queryStringDictionary.Count;

        string IReadOnlyDictionary<string, string>.this[string key] => queryStringDictionary[key];
    }
}
