using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Extensions;
using DNI.Core.Shared.Attributes;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations.Cache
{
    [IgnoreScanning]
    public class DistributedCacheService : AsyncCacheServiceBase
    {
        public DistributedCacheService(
            IMemoryStreamProvider memoryStreamProvider,
            IExceptionHandler exceptionHandler,
            IDistributedCache distributedCache,
            DistributedCacheEntryOptions distributedCacheEntryOptions,
            MessagePackSerializerOptions messagePackSerializerOptions)
            : base(memoryStreamProvider, messagePackSerializerOptions)
        {
            this.exceptionHandler = exceptionHandler;
            this.distributedCache = distributedCache;
            this.distributedCacheEntryOptions = distributedCacheEntryOptions;
        }

        public override async Task<IAttempt<T>> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            return await exceptionHandler.TryAsync(key, async (key) =>
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var value = await distributedCache.GetAsync(key);

                if (value == null || value.Length < 0)
                {
                    throw new NullReferenceException($"Cache value for '{key}' not available");
                }

                return CreateAttempt(await DeserializeAsync<T>(value, cancellationToken));
            }, (exception) =>
            {
                var attempt = CreateAttempt<T>(exception);
                return Task.FromResult(attempt);
            }, exceptionTypes => exceptionTypes
               .DescribeType<NullReferenceException>()
               .DescribeType<MessagePackSerializationException>());
        }

        public override async Task<IAttempt<T>> SetAsync<T>(string key, T value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return await exceptionHandler.TryAsync(key, async (key) =>
            {
                var serializedValue = await SerializeAsync(value, cancellationToken);
                await distributedCache.SetAsync(key, serializedValue.ToArray(), distributedCacheEntryOptions, cancellationToken);
                return CreateAttempt(value);
            }, exception => Task.FromResult(CreateAttempt<T>(exception)), exceptionTypes => exceptionTypes.DescribeType<NullReferenceException>()
                .DescribeType<MessagePackSerializationException>());

        }

        public override bool TryGet<T>(string key, out T value)
        {
            value = default;
            var task = GetAsync<T>(key, CancellationToken.None);
                task.ConfigureAwait(true);

            task.Wait(CancellationToken.None);

            if (task.Result.Successful)
            {
                value = task.Result.Result;
                return true;
            }

            return false;
        }

        public override bool TrySet<T>(string key, T value)
        {
            var task = SetAsync(key, value, CancellationToken.None);
            task.ConfigureAwait(true);

            task.Wait(CancellationToken.None);
            if (task.Result.Successful)
            {
                return true;
            }

            return false;
        }

        public override Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            return distributedCache.RemoveAsync(key, cancellationToken);
        }

        public override void Remove(string key)
        {
            var task = RemoveAsync(key, CancellationToken.None);
            task.Wait();
        }

        private readonly IExceptionHandler exceptionHandler;
        private readonly IDistributedCache distributedCache;
        private readonly DistributedCacheEntryOptions distributedCacheEntryOptions;
    }
}
