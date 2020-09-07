using DNI.Core.Contracts.Providers;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DNI.Core.Services.Providers
{
    public class MemoryStreamProvider : IMemoryStreamProvider
    {
        public MemoryStreamProvider(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            this.recyclableMemoryStreamManager  = recyclableMemoryStreamManager;
        }

        public MemoryStream GetMemoryStream(Guid id, string reference)
        {
            return recyclableMemoryStreamManager.GetStream(id, reference);
        }

        public MemoryStream GetMemoryStream(Guid id, string reference, IEnumerable<byte> initialValue)
        {
            var memoryStream = GetMemoryStream(id, reference);

            var initialValueArray = initialValue.ToArray();
            memoryStream.Write(initialValueArray, 0 , initialValueArray.Length);

            return memoryStream;
        }

        public MemoryStream GetMemoryStream()
        {
            return new MemoryStream();
        }

        public MemoryStream GetMemoryStream(IEnumerable<byte> initialValue)
        {
            return new MemoryStream(initialValue.ToArray());
        }

        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    }
}
