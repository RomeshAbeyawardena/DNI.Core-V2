using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Builders;
using DNI.Core.Shared.Enumerations;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class JwtService : IJwtService
    {
        public JwtSecurityToken BuildJwtSecurityToken(
            Action<JwtHeader> buildHeader, 
            Action<JwtPayload> buildPayload)
        {
            var header = new JwtHeader();
            var payload = new JwtPayload();

            buildHeader(header);
            buildPayload(payload);

            return new JwtSecurityToken(header, payload);
        }

        public EncryptingCredentials GetEncryptingCredentials(
            EncryptionCredentialType encryptionCredentialType, 
            IEncryptionProfile encryptionProfile)
        {
            return encryptionCredentialType switch
            {
                EncryptionCredentialType.SymmetricSecurityKey => new EncryptingCredentials(
                    new SymmetricSecurityKey(encryptionProfile.Key.ToArray()), nameof(Aes)),
                _ => throw new NotImplementedException(),
            };
        }

        public IEnumerable<Claim> BuildClaims(
            Action<IClaimsBuilder> buildClaims, 
            IEnumerable<Claim> claims = null)
        {
            var claimsBuilder = new ClaimsBuilder(claims);
            buildClaims(claimsBuilder);
            return claimsBuilder.ToArray();
        }
    }
}
