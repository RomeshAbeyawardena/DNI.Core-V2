using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IAsyncCacheService : ICacheService
    {
        Task<IAttempt<T>> GetAsync<T>(string key, CancellationToken cancellationToken)
            where T: class;

        Task<IAttempt<T>> SetAsync<T>(string key, T value, CancellationToken cancellationToken)
            where T: class;

        Task RemoveAsync(string key, CancellationToken cancellationToken);
    }
}
