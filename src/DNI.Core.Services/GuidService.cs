using DNI.Core.Contracts.Services;
using System;

namespace DNI.Core.Services
{
    public class GuidService : IGuidService
    {
        public Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}
