using DNI.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using DNI.Core.Services.Abstractions;
using DNI.Core.Shared.Attributes;

namespace DNI.Core.Services.Implementations.Data
{
    [IgnoreScanning]
    internal class AsyncEntityFrameworkRepository<TDbContext, TEntity> : 
        EntityFrameworkRepository<TDbContext, TEntity>, 
        IAsyncRepository<TEntity>
        where TDbContext : EnhancedDbContextBase
        where TEntity : class
    {
        public AsyncEntityFrameworkRepository(TDbContext dbContext, IRepositoryOptions repositoryOptions) 
            : base(dbContext, repositoryOptions)
        {
        }

        public Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if(TryRemove(entity, out var entry)){   
                entry.State = EntityState.Deleted;
                return DbContext.SaveChangesAsync(cancellationToken);
            }

            return Task.FromResult(Core.Shared.Constants.Data.DatabaseOperationFailed);
        }

        public Task<TEntity> FindAsync(CancellationToken cancellationToken = default, params object[] keys)
        {
            return DbSet.FindAsync(keys, cancellationToken).AsTask();
        }

        public Task<int> SaveChangesAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            AddOrUpdate(entity);
            return DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
