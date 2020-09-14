using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using MessagePack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class AsyncCacheServiceBase : CacheServiceBase, IAsyncCacheService
    {
        protected AsyncCacheServiceBase(IMemoryStreamProvider memoryStreamProvider,
            IJsonStreamSerializerProvider jsonStreamSerializerProvider,
            MessagePackSerializerOptions messagePackSerializerOptions) 
            : base(memoryStreamProvider, 
                  jsonStreamSerializerProvider,
                  messagePackSerializerOptions)
        {
        }

        public abstract Task RemoveAsync(string key, CancellationToken cancellationToken);

        public abstract Task<IAttempt<T>> GetAsync<T>(string key, CancellationToken cancellationToken, bool useMessagePack = true)
            where T: class;

        public abstract Task<IAttempt<T>> SetAsync<T>(string key, T value, CancellationToken cancellationToken, bool useMessagePack = true)
            where T: class;

        protected static IAttempt<T> CreateAttempt<T>(Exception exception)
            where T: class
        {
            return Attempt.Create<T>(exception);
        }

        protected static IAttempt<T> CreateAttempt<T>(T result)
            where T: class
        {
            return Attempt.Create(result);
        }
    }
}
