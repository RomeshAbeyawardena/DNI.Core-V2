using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class CacheStateItem<TState> : ICacheStateItem<TState>
    {
        public static implicit operator KeyValuePair<string, TState>(CacheStateItem<TState> value)
        {
            return KeyValuePair.Create(value.Key, value.State);
        }

        public static implicit operator CacheStateItem<TState>(KeyValuePair<string, TState> value)
        {
            return new CacheStateItem<TState>
            {
                Key = value.Key,
                State = value.Value
            };
        }

        public string Key { get; set; }
        public TState State  { get; set; }

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
    }
}
