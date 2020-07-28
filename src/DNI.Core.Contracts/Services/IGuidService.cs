using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IGuidService
    {
        Guid GenerateGuid();
    }
}
