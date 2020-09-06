using System.Security.Claims;

namespace DNI.Core.Contracts.Builders
{
    public interface IClaimsBuilder : IListBuilder<Claim>
    {
        new IClaimsBuilder Add(Claim claim);
    }
}
