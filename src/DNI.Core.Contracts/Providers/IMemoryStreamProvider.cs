using System;
using System.Collections.Generic;
using System.IO;

namespace DNI.Core.Contracts.Providers
{
    public interface IMemoryStreamProvider
    {
        MemoryStream GetMemoryStream();
        MemoryStream GetMemoryStream(IEnumerable<byte> initialValue);
        MemoryStream GetMemoryStream(Guid id, string reference);
        MemoryStream GetMemoryStream(Guid id, string reference, IEnumerable<byte> initialValue);
    }
}
