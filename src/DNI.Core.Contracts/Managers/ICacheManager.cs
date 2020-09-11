using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Services;
using DNI.Core.Shared.Enumerations;

namespace DNI.Core.Contracts.Managers
{
    public interface ICacheManager : IImplementationServiceFactory<CacheType, ICacheService>
    {
        ICacheService GetCacheService(CacheType cacheType);
        IAsyncCacheService GetAsyncCacheService(CacheType cacheType);
    }
}
