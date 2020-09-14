using DNI.Core.Contracts.Providers;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    public sealed class JsonStreamSerializerProvider : IJsonStreamSerializerProvider
    {
        public JsonStreamSerializerProvider(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public Task<T> DeserializeStreamAsync<T>(Stream stream, CancellationToken cancellationToken)
        {
            streamReader = new StreamReader(stream);
            jsonTextReader = new JsonTextReader(streamReader);
            return Task.FromResult(jsonSerializer.Deserialize<T>(jsonTextReader));
        }

        public async Task SerializeStreamAsync<T>(Stream stream, T value, CancellationToken cancellationToken)
        {
            streamWriter = new StreamWriter(stream);
            jsonSerializer.Serialize(streamWriter, value);
            await streamWriter.FlushAsync();
        }

        public void Dispose()
        {
            streamWriter?.Dispose();
            streamReader?.DiscardBufferedData();
            streamReader?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if(streamWriter != null)
            { 
                await streamWriter.DisposeAsync();
            }

            Dispose();
        }

        private readonly JsonSerializer jsonSerializer;
        private StreamReader streamReader;
        private JsonTextReader jsonTextReader;
        private StreamWriter streamWriter;
    }

}
