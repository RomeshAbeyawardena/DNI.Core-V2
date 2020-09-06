using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DNI.Core.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Saves changes to store inserting or updating <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int SaveChanges(TEntity entity, bool detachAfterCommit = true);

        /// <summary>
        /// Deletes <see cref="TEntity"/>
        /// </summary>
        int Delete(TEntity entity);

        /// <summary>
        /// Finds a value from the store
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        TEntity Find(params object[] keys);

        /// <summary>
        /// Returns the IQueryable <see cref="TEntity"/> object available by the store
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        /// Returns the last entity state
        /// </summary>
        EntityState LastEntityState { get; }
    }
}
