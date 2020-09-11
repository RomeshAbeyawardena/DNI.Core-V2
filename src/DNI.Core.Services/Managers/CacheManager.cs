using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Abstractions;
using DNI.Core.Shared.Enumerations;
using System;
using System.Collections.Generic;

namespace DNI.Core.Services.Managers
{
    public class CacheManager : ImplementationServiceFactoryBase<CacheType, ICacheService>, ICacheManager
    {
        public CacheManager(IServiceProvider serviceProvider, IEnumerable<KeyValuePair<CacheType, Type>> cacheServices)
            : base(serviceProvider, cacheServices)
        {
            
        }

        public IAsyncCacheService GetAsyncCacheService(CacheType cacheType)
        {
            return GetCacheService(cacheType) as IAsyncCacheService;
        }

        public ICacheService GetCacheService(CacheType cacheType)
        {
            if (TryGetImplementation(cacheType, out var cacheService))
            {
                return cacheService;
            }

            return default;
        }
    }
}
