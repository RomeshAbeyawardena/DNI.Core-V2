using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IJsonStreamSerializerProvider : IDisposable, IAsyncDisposable
    {
        Task<T> DeserializeStreamAsync<T>(Stream stream, CancellationToken cancellationToken);
        Task SerializeStreamAsync<T>(Stream stream, T value, CancellationToken cancellationToken);
    }
}
