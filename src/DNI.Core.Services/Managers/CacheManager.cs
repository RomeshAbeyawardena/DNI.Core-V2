using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Implementations.Cache;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Enumerations;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Managers
{
    public class CacheManager : ICacheManager
    {
        public CacheManager(IServiceProvider serviceProvider, IEnumerable<KeyValuePair<CacheType, Type>> cacheServices)
        {
            this.serviceProvider = serviceProvider;
            if(cacheServices != null)
            {
                dictionary = new ConcurrentDictionary<CacheType, Type>(cacheServices);
                return;
            }
        }

        Type IReadOnlyDictionary<CacheType, Type>.this[CacheType key] => dictionary[key];

        IEnumerable<CacheType> IReadOnlyDictionary<CacheType, Type>.Keys => dictionary.Keys;

        IEnumerable<Type> IReadOnlyDictionary<CacheType, Type>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<CacheType, Type>>.Count => dictionary.Count;

        public IAsyncCacheService GetAsyncCacheService(CacheType cacheType)
        {
            return GetCacheService(cacheType) as IAsyncCacheService;
        }

        public ICacheService GetCacheService(CacheType cacheType)
        {
            if(TryGetValue(cacheType, out var cacheService))
            {
                var type = Type.GetType(cacheService.FullName);
                var s =  serviceProvider.GetService(type);
                return s as ICacheService;
            }

            return default;
        }

        bool IReadOnlyDictionary<CacheType, Type>.ContainsKey(CacheType key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<CacheType, Type>> IEnumerable<KeyValuePair<CacheType, Type>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public bool TryGetValue(CacheType key, out Type value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private readonly ConcurrentDictionary<CacheType, Type> dictionary;
        private readonly IServiceProvider serviceProvider;
    }
}
