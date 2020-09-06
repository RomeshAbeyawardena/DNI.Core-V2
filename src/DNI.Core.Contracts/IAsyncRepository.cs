using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IAsyncRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<int> SaveChangesAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(CancellationToken cancellationToken = default, params object[] keys);

        Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
