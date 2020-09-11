using System;
using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface ICacheState<TState> : IReadOnlyDictionary<string, TState>
    {
        bool TryAddOrUpdate(ICacheStateItem<TState> cacheStateItem);
        IDisposable OnStageItemChanged(Action<ICacheStateItem<TState>> onValueChanged);
    }
}
