using Microsoft.IdentityModel.Tokens;

namespace DNI.Core.Contracts.Providers
{
    public interface ITokenHandlerProvider
    {
        SecurityTokenHandler GetSecurityTokenHandler();
    }
}
