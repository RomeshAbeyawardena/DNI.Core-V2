using DNI.Core.Contracts;
using DNI.Core.Services.Abstractions;
using DNI.Core.Shared;
using DNI.Core.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations.Data
{
    internal class EntityFrameworkRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : EnhancedDbContextBase
        where TEntity : class
    {
        public EntityFrameworkRepository(TDbContext dbContext, IRepositoryOptions repositoryOptions)
        {
            if(dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if(repositoryOptions == null)
            {
                throw new ArgumentNullException(nameof(repositoryOptions));
            }

            this.repositoryOptions = repositoryOptions;
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query => repositoryOptions.EnableTracking
            ? DbSet 
            : DbSet.AsNoTracking();

        public TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public int SaveChanges(TEntity entity, bool detachAfterCommit = true)
        {
            AddOrUpdate(entity);
            var result = DbContext.SaveChanges();

            if (detachAfterCommit)
            {
                DbContext.Entry(entity).State = EntityState.Detached;
            }

            return result;
        }

        public int Delete(TEntity entity)
        {
            if(TryRemove(entity, out var entry)){   
                entry.State = EntityState.Deleted;
                return DbContext.SaveChanges();
            }

            return Core.Shared.Constants.Data.DatabaseOperationFailed;
        }

        protected void SetEntityStateByPrimaryKeyExistanceOfEntity(TEntity entity, out EntityEntry<TEntity> entry)
        {
            var entityType = typeof(TEntity);
            entry = DbContext.Entry(entity);

            var primaryKey = DbContext.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey();

            if(primaryKey == null)
            {
                entry.State = EntityState.Added;
            }

            foreach (var property in primaryKey.Properties)
            {
                var entityProperty = entityType.GetProperty(property.Name);
                var entityPropertyValue = entityProperty.GetValue(entity);

                if(entityPropertyValue.IsDefault())
                {
                    entry.State = EntityState.Added;
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }

            DbContext.ReportChange(entry);
            LastEntityState = entry.State;
        }

        protected EntityEntry<TEntity> AddOrUpdate(TEntity entity)
        {
            SetEntityStateByPrimaryKeyExistanceOfEntity(entity, out var entry);
            return entry;
        }

        protected bool TryRemove(TEntity entity, out EntityEntry<TEntity> entry)
        {
            SetEntityStateByPrimaryKeyExistanceOfEntity(entity, out entry);

            if(entry.State != EntityState.Modified)
            { 
                return false;
            }


            entry.State = EntityState.Deleted;
            return true;
        }

        protected DbSet<TEntity> DbSet { get; }
        
        protected TDbContext DbContext { get; }

        public EntityState LastEntityState { get; private set; }

        private readonly IRepositoryOptions repositoryOptions;
    }
}
