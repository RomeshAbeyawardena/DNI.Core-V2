using DNI.Core.Contracts.Builders;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Builders
{
    [IgnoreScanning]
    public class ClaimsBuilder : ListBuilder<Claim>, IClaimsBuilder
    {
        public ClaimsBuilder(IEnumerable<Claim> claims = null)
            : base(claims)
        {

        }

        IClaimsBuilder IClaimsBuilder.Add(Claim claim)
        {
            Add(claim);
            return this;
        }
    }
}
