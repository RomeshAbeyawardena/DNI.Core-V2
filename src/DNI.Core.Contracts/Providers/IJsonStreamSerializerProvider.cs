using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IJsonStreamSerializerProvider
    {
        Task<T> DeserializeStreamAsync<T>(Stream stream);
        Task SerializeStreamAsync<T>(Stream stream, T value);
    }
}
