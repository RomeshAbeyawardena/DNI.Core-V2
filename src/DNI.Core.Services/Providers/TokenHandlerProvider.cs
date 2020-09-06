using DNI.Core.Contracts.Providers;
using Microsoft.IdentityModel.Tokens;

namespace DNI.Core.Services.Providers
{
    public class TokenHandlerProvider : ITokenHandlerProvider
    {
        private readonly ISecurityTokenValidator securityTokenValidator;

        public TokenHandlerProvider(ISecurityTokenValidator securityTokenValidator)
        {
            this.securityTokenValidator = securityTokenValidator;
        }

        public SecurityTokenHandler GetSecurityTokenHandler()
        {
            return securityTokenValidator as SecurityTokenHandler;
        }
    }
}
