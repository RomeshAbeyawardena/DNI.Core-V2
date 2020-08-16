using DNI.Core.Contracts.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Builders
{
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
