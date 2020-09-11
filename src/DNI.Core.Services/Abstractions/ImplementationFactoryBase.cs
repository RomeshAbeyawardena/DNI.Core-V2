using DNI.Core.Contracts.Factories;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ImplementationFactoryBase<TKey, TValue> : IImplementationFactory<TKey, TValue>
    {
        protected ImplementationFactoryBase(IEnumerable<KeyValuePair<TKey, TValue>> dictionaryProvider)
        {
            if(dictionaryProvider != null)
            {
                Dictionary = new ConcurrentDictionary<TKey, TValue>(dictionaryProvider);
                return;
            }

            Dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => Dictionary[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => Dictionary.Count;

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

        protected ConcurrentDictionary<TKey, TValue> Dictionary { get; }
        
    }

    public abstract class ImplementationServiceFactoryBase<TKey, TGenericImplementation> : ImplementationFactoryBase<TKey, Type>,  
        IImplementationServiceFactory<TKey, TGenericImplementation>
    {
        protected ImplementationServiceFactoryBase(IServiceProvider serviceProvider,
            IEnumerable<KeyValuePair<TKey, Type>> dictionaryProvider)
            : base(dictionaryProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        public bool TryGetImplementation(TKey key, out TGenericImplementation implementation)
        {
            implementation = default;
            if(Dictionary.TryGetValue(key, out var serviceType))
            {
                implementation = (TGenericImplementation) ServiceProvider.GetService(serviceType);
                return true;
            }

            return false;
        }


        protected IServiceProvider ServiceProvider { get; }
    }
}
