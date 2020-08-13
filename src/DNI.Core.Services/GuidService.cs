using DNI.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
