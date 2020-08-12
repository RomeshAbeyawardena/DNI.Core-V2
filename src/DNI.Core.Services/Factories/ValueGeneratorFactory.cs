using DNI.Core.Contracts.Factories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Factories
{
    internal class ValueGeneratorFactory : IValueGeneratorFactory
    {
        public ValueGeneratorFactory(IEnumerable<KeyValuePair<string, Type>> valueKeyPairs)
        {
            if(valueKeyPairs == null)
            throw new ArgumentNullException(nameof(valueKeyPairs));

            _dictionary = new Dictionary<string, Type>();

            foreach(var valueKeyPair in valueKeyPairs)
            { 
                _dictionary.Add(valueKeyPair);
            }
        }

        Type IReadOnlyDictionary<string, Type>.this[string key] => _dictionary[key];

        IEnumerable<string> IReadOnlyDictionary<string, Type>.Keys => _dictionary.Keys;

        IEnumerable<Type> IReadOnlyDictionary<string, Type>.Values => _dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<string, Type>>.Count => _dictionary.Count;

        bool IReadOnlyDictionary<string, Type>.ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<string, Type>> IEnumerable<KeyValuePair<string, Type>>.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return  _dictionary.GetEnumerator();
        }

        bool IReadOnlyDictionary<string, Type>.TryGetValue(string key, out Type value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        private readonly IDictionary<string, Type> _dictionary;
    }
}
