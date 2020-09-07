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
            MessagePackSerializerOptions messagePackSerializerOptions) 
            : base(memoryStreamProvider, messagePackSerializerOptions)
        {
        }

        public abstract Task<IAttempt<T>> GetAsync<T>(string key, CancellationToken cancellationToken)
            where T: class;

        public abstract Task<IAttempt<T>> SetAsync<T>(string key, T value, CancellationToken cancellationToken)
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
