using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Managers
{
    public interface IDatabaseEntityManager<TEntity> : IDatabaseEntityManager
        where TEntity: class
    {
        new IDapperContext<TEntity> Context { get; }
    }

    public interface IDatabaseEntityManager : IDisposable
    {
        IDapperContext Context { get; }
    }
}
