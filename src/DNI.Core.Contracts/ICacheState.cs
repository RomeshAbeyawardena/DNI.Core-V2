using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ICacheState<TState> : IReadOnlyDictionary<string, TState>
    {
        bool TryAddOrUpdate(ICacheStateItem<TState> cacheStateItem);
    }
}
