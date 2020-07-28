﻿using DNI.Core.Contracts;
using DNI.Core.Shared;
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
        where TDbContext : DbContext
        where TEntity : class
    {
        protected EntityFrameworkRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query => DbSet;

        public TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public int SaveChanges(TEntity entity)
        {
            AddOrUpdate(entity);
            return DbContext.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            if(TryRemove(entity, out var entry)){   
                entry.State = EntityState.Deleted;
                return DbContext.SaveChanges();
            }

            return Shared.Constants.Data.DatabaseOperationFailed;
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
                if(entityProperty.GetValue(entity) == default(TEntity))
                {
                    entry.State = EntityState.Added;
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
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

            return true;
        }

        protected DbSet<TEntity> DbSet { get; }
        
        protected TDbContext DbContext { get; }
    }
}
