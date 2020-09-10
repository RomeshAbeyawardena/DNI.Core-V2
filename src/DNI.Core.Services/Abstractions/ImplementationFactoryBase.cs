using DNI.Core.Contracts.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ImplementationFactoryBase<TKey, TValue> : IImplementationFactory<TKey, TValue>
    {
        protected ImplementationFactoryBase(IEnumerable<KeyValuePair<TKey, TValue>> dictionaryProvider)
        {
            if(dictionaryProvider != null)
            {
                dictionary = new ConcurrentDictionary<TKey, TValue>(dictionaryProvider);
                return;
            }

            dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => dictionary[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => dictionary.Count;

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

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private readonly ConcurrentDictionary<TKey, TValue> dictionary;
        
    }

    public abstract class ImplementationServiceFactoryBase<TKey, TGenericImplementation> : ImplementationFactoryBase<TKey, Type>,  
        IImplementationServiceFactory<TKey, TGenericImplementation>
    {
        protected ImplementationServiceFactoryBase(IServiceProvider serviceProvider,
            IEnumerable<KeyValuePair<TKey, Type>> dictionaryProvider)
            : base(dictionaryProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        
        
        public bool TryGetImplementation(TKey key, out TGenericImplementation implementation)
        {
            implementation = default;
            if(TryGetValue(key, out var serviceType))
            {
                implementation = (TGenericImplementation) serviceProvider.GetService(serviceType);
                return true;
            }

            return false;
        }


        private readonly IServiceProvider serviceProvider;
    }
}
