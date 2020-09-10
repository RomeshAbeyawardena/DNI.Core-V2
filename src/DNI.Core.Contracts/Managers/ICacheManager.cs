using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Services;
using DNI.Core.Shared.Enumerations;
using System;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Managers
{
    public interface ICacheManager : IImplementationServiceFactory<CacheType, ICacheService>
    {
        ICacheService GetCacheService(CacheType cacheType);
        IAsyncCacheService GetAsyncCacheService(CacheType cacheType);
    }
}
