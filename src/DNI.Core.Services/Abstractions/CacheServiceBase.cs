using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using MessagePack;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class CacheServiceBase : ICacheService
    {
        protected CacheServiceBase(IMemoryStreamProvider memoryStreamProvider,
            MessagePackSerializerOptions messagePackSerializerOptions)
        {
            this.memoryStreamProvider = memoryStreamProvider;
            this.messagePackSerializerOptions = messagePackSerializerOptions;
        }

        public abstract bool TryGet<T>(string key, out T Value)
            where T: class;

        public abstract bool TrySet<T>(string key, T value)
            where T: class;

        public abstract void Remove(string key);

        protected async Task<IEnumerable<byte>> SerializeAsync<T>(T value, CancellationToken cancellationToken)
        {
            using (var messageStream = memoryStreamProvider.GetMemoryStream())
            { 
                await MessagePackSerializer.SerializeAsync(messageStream, value, messagePackSerializerOptions, cancellationToken);
                return messageStream.ToArray();
            }
        }
        
        protected async Task<T> DeserializeAsync<T>(IEnumerable<byte> value, CancellationToken cancellationToken)
        {
            using( var memoryStream = memoryStreamProvider.GetMemoryStream(value))
            { 
                return await MessagePackSerializer.DeserializeAsync<T>(memoryStream, messagePackSerializerOptions, cancellationToken);
            }
        }

        private readonly IMemoryStreamProvider memoryStreamProvider;
        private readonly MessagePackSerializerOptions messagePackSerializerOptions;
    }
}
