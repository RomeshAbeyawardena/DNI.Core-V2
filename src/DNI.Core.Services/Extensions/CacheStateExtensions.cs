using DNI.Core.Contracts;
using DNI.Core.Domains;
using Microsoft.Extensions.Internal;
using System;

namespace DNI.Core.Services.Extensions
{
    public static class CacheStateExtensions
    {
        public static void Set(this ICacheState<DateTime> cacheState, string key, ISystemClock systemClock, TimeSpan validTimeSpan)
        {
            cacheState.TryAddOrUpdate(CacheStateItem.Create(key, systemClock.UtcNow.Add(validTimeSpan).UtcDateTime));
        }

        public static void Set(this ICacheState<DateTimeOffset> cacheState, string key, ISystemClock systemClock, TimeSpan validTimeSpan)
        {
            cacheState.TryAddOrUpdate(CacheStateItem.Create(key, systemClock.UtcNow.Add(validTimeSpan)));
        }

        public static bool IsValid(this ICacheState<DateTimeOffset> cacheState, string key, ISystemClock systemClock)
        {
            if(cacheState.TryGetValue(key, out var validFrom))
            {
                return validFrom < systemClock.UtcNow;
            }

            return false;
        }

        public static bool IsValid(this ICacheState<DateTime> cacheState, string key, ISystemClock systemClock)
        {
            if(cacheState.TryGetValue(key, out var validFrom))
            {
                return validFrom < systemClock.UtcNow;
            }

            return false;
        }
    }
}
