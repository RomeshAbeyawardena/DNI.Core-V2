using DNI.Core.Contracts.Services;

namespace DNI.Core.Contracts.Managers
{
    public interface ICacheManager
    {
        ICacheService GetCacheService();
    }
}
