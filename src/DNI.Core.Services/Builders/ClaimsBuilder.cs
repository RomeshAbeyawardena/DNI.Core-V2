using DNI.Core.Contracts.Builders;
using DNI.Core.Shared.Attributes;
using System.Collections.Generic;
using System.Security.Claims;

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
