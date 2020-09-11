using DNI.Core.Contracts;
using System;
using System.Collections.Generic;

namespace DNI.Core.Domains
{
    public static class CacheStateItem
    {
        public static ICacheStateItem<TState> Create<TState>(string key, TState state)
        {
            return new CacheStateItem<TState>(key, state);
        }
    }

    [MessagePack.MessagePackObject(true)]
    public class CacheStateItem<TState> : ICacheStateItem<TState>
    {
        public static implicit operator KeyValuePair<string, TState>(CacheStateItem<TState> value)
        {
            return KeyValuePair.Create(value.Key, value.State);
        }

        public static implicit operator CacheStateItem<TState>(KeyValuePair<string, TState> value)
        {
            return new CacheStateItem<TState>(value.Key, value.Value);
        }

        public string Key { get; }
        public TState State  { get; }

        public override bool Equals(object obj)
        {
            if(obj is CacheStateItem<TState> cacheItemState)
            {
                return Equals(cacheItemState);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, State);
        }

        public bool Equals(CacheStateItem<TState> cacheStateItem)
        {
            return cacheStateItem.Key == Key
                && cacheStateItem.State.Equals(State);
        }

        internal CacheStateItem(string key, TState state)
        {
            Key = key;
            State = state;
        }
    }
}
