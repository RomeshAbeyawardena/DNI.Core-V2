using DNI.Core.Contracts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Managers
{
    internal class ValueGeneratorManager : IValueGeneratorManager
    {
        public ValueGeneratorManager(IEnumerable<KeyValuePair<string, Type>> valueKeyPairs)
        {
            if(valueKeyPairs == null)
            throw new ArgumentNullException(nameof(valueKeyPairs));

            dictionary = new Dictionary<string, Type>();

            foreach(var valueKeyPair in valueKeyPairs)
            { 
                dictionary.Add(valueKeyPair);
            }
        }

        Type IReadOnlyDictionary<string, Type>.this[string key] => dictionary[key];

        IEnumerable<string> IReadOnlyDictionary<string, Type>.Keys => dictionary.Keys;

        IEnumerable<Type> IReadOnlyDictionary<string, Type>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<string, Type>>.Count => dictionary.Count;

        bool IReadOnlyDictionary<string, Type>.ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<string, Type>> IEnumerable<KeyValuePair<string, Type>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return  dictionary.GetEnumerator();
        }

        bool IReadOnlyDictionary<string, Type>.TryGetValue(string key, out Type value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private readonly IDictionary<string, Type> dictionary;
    }
}
