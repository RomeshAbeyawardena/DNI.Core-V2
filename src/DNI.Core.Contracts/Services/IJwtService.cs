using DNI.Core.Contracts.Builders;
using DNI.Core.Shared.Enumerations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IJwtService
    {
        JwtSecurityToken BuildJwtSecurityToken(Action<JwtHeader> buildHeader, Action<JwtPayload> buildPayload);
        EncryptingCredentials GetEncryptingCredentials(EncryptionCredentialType encryptionCredentialType, IEncryptionProfile encryptionProfile);
        IEnumerable<Claim> BuildClaims(Action<IClaimsBuilder> buildClaims, IEnumerable<Claim> claims = null);
        string GenerateToken(JwtSecurityToken token);
        bool TryReadToken(
            string token, 
            TokenValidationParameters tokenValidationParameters, 
            out SecurityToken securityToken,
            out ClaimsPrincipal claimsPrincipal);
    }
}
