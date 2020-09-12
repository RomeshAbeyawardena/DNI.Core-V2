using DNI.Core.Contracts.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    public class JsonStreamSerializerProvider : IJsonStreamSerializerProvider
    {
        public JsonStreamSerializerProvider(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public Task<T> DeserializeStreamAsync<T>(Stream stream)
        {
            var dataReader = new BsonDataReader(stream);
            return Task.FromResult(jsonSerializer.Deserialize<T>(dataReader));
        }

        public Task SerializeStreamAsync<T>(Stream stream, T value)
        {
            var dataWriter = new BsonDataWriter(stream);
            jsonSerializer.Serialize(dataWriter, value);
            return Task.CompletedTask;
        }

        private readonly JsonSerializer jsonSerializer;
    }
}
