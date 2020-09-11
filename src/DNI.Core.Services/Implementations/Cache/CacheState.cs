using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace DNI.Core.Services.Implementations.Cache
{
    [IgnoreScanning]
    public class CacheState<TState> : ICacheState<TState>
    {
        public CacheState(ISubject<ICacheStateItem<TState>> cacheStateItemSubject)
        {
            dictionary = new ConcurrentDictionary<string, TState>();
            this.cacheStateItemSubject = cacheStateItemSubject;
        }

        TState IReadOnlyDictionary<string, TState>.this[string key] => dictionary[key];

        IEnumerable<string> IReadOnlyDictionary<string, TState>.Keys => dictionary.Keys;

        IEnumerable<TState> IReadOnlyDictionary<string, TState>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<string, TState>>.Count => dictionary.Count;

        bool IReadOnlyDictionary<string, TState>.ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<string, TState>> IEnumerable<KeyValuePair<string, TState>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        bool IReadOnlyDictionary<string, TState>.TryGetValue(string key, out TState value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public bool TryAddOrUpdate(ICacheStateItem<TState> cacheStateItem)
        {
            if(dictionary.TryAdd(cacheStateItem.Key, cacheStateItem.State))
            {
                cacheStateItemSubject.OnNext(cacheStateItem);
                return true;
            }

            if(dictionary.TryGetValue(cacheStateItem.Key, out var state))
            {
                if(dictionary.TryUpdate(cacheStateItem.Key, cacheStateItem.State, state))
                {
                    cacheStateItemSubject.OnNext(cacheStateItem);
                    return true;
                }
            }
            
            return false;    
        }

        public IDisposable OnStageItemChanged(Action<ICacheStateItem<TState>> onValueChanged)
        {
            return cacheStateItemSubject.Subscribe(onValueChanged);
        }

        private readonly ConcurrentDictionary<string, TState> dictionary;
        private readonly ISubject<ICacheStateItem<TState>> cacheStateItemSubject;
    }
}
