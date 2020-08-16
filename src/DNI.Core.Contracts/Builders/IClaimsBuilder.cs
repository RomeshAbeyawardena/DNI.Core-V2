using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Builders
{
    public interface IClaimsBuilder : IListBuilder<Claim>
    {
        new IClaimsBuilder Add(Claim claim);
    }
}
