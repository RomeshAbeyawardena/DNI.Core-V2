using System.Collections.Generic;

namespace MultiStackServiceHost.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary.Count > 0;
        }
    }
}
