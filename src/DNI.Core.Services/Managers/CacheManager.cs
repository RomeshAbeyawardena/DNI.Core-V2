using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Abstractions;
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
