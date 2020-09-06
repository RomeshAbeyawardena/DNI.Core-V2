using DNI.Core.Contracts.Services;

namespace DNI.Core.Services.Abstractions
{
    public abstract class CacheServiceBase : ICacheService
    {
        protected CacheServiceBase()
        {

        }

        public abstract bool TryGet<T>(string key, out T Value);

        public abstract bool TrySet<T>(T value);
    }
}
