using DNI.Core.Contracts;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Extensions;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations.Cache
{
    public class DistributedCacheService : AsyncCacheServiceBase
    {
        public DistributedCacheService(
            IExceptionHandler exceptionHandler,
            IDistributedCache distributedCache,
            MessagePackSerializerOptions messagePackSerializer)
        {
            this.exceptionHandler = exceptionHandler;
            this.distributedCache = distributedCache;
            messagePackSerializerOptions = messagePackSerializer;
        }

        public override async Task<IAttempt<T>> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            return await exceptionHandler.TryAsync<string, IAttempt<T>>(key, async (key) =>
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var value = await distributedCache.GetAsync(key);

                if (value == null)
                {
                    throw new NullReferenceException($"Cache value for '{key}' not available");
                }

                return CreateAttempt(MessagePackSerializer.Deserialize<T>(value, messagePackSerializerOptions));
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

            return await exceptionHandler.TryAsync<string, IAttempt<T>>(key, async (key) =>
            {
                using var messageStream = new MemoryStream();
                await MessagePackSerializer.SerializeAsync(messageStream, value, messagePackSerializerOptions, cancellationToken);
                messageStream.Position = 0;

                await distributedCache.SetAsync(key, messageStream.ToArray());
                return CreateAttempt(value);
            }, exception => Task.FromResult(CreateAttempt<T>(exception)), exceptionTypes => exceptionTypes.DescribeType<NullReferenceException>()
                .DescribeType<MessagePackSerializationException>());

        }

        public override bool TryGet<T>(string key, out T Value)
        {
            throw new NotImplementedException();
        }

        public override bool TrySet<T>(T value)
        {
            throw new NotImplementedException();
        }

        private readonly IExceptionHandler exceptionHandler;
        private readonly IDistributedCache distributedCache;
        private readonly MessagePackSerializerOptions messagePackSerializerOptions;
    }
}
