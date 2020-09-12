using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using MessagePack;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class CacheServiceBase : ICacheService
    {
        protected CacheServiceBase(IMemoryStreamProvider memoryStreamProvider,
            IJsonStreamSerializerProvider jsonStreamSerializerProvider,
            MessagePackSerializerOptions messagePackSerializerOptions)
        {
            this.memoryStreamProvider = memoryStreamProvider;
            this.jsonStreamSerializerProvider = jsonStreamSerializerProvider;
            this.messagePackSerializerOptions = messagePackSerializerOptions;
        }

        public abstract bool TryGet<T>(string key, out T Value)
            where T: class;

        public abstract bool TrySet<T>(string key, T value)
            where T: class;

        public abstract void Remove(string key);

        protected async Task<IEnumerable<byte>> SerializeAsync<T>(T value, CancellationToken cancellationToken, bool useMessagePack = true)
        {
            using (var messageStream = memoryStreamProvider.GetMemoryStream())
            { 
                if(useMessagePack)
                { 
                    await MessagePackSerializer.SerializeAsync(messageStream, value, messagePackSerializerOptions, cancellationToken);
                }
                else
                { 
                    await jsonStreamSerializerProvider.SerializeStreamAsync(messageStream, value, cancellationToken);
                }

                return messageStream.ToArray();
            }
        }
        
        protected async Task<T> DeserializeAsync<T>(IEnumerable<byte> value, CancellationToken cancellationToken, bool useMessagePack = true)
        {
            using( var memoryStream = memoryStreamProvider.GetMemoryStream(value))
            { 
                if(useMessagePack)
                { 
                    return await MessagePackSerializer.DeserializeAsync<T>(memoryStream, messagePackSerializerOptions, cancellationToken);
                }
                else
                {
                    return await jsonStreamSerializerProvider.DeserializeStreamAsync<T>(memoryStream, cancellationToken);
                }

            }
        }

        private readonly IMemoryStreamProvider memoryStreamProvider;
        private readonly IJsonStreamSerializerProvider jsonStreamSerializerProvider;
        private readonly MessagePackSerializerOptions messagePackSerializerOptions;
    }
}
