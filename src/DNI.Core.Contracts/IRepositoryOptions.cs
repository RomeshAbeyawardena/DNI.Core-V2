using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IRepositoryOptions
    {
        bool EnableTracking { get; set; }
    }
}
