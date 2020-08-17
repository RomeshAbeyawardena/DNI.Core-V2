using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Definitions;
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
        public JwtService(IExceptionHandler exceptionHandler)
        {
            lazyJwtSecurityTokenHandler = new Lazy<JwtSecurityTokenHandler>(() => new JwtSecurityTokenHandler(), true);
            this.exceptionHandler = exceptionHandler;
        }

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

        public string GenerateToken(JwtSecurityToken token)
        {
            return JwtSecurityTokenHandler.WriteToken(token);
        }

        public bool TryReadToken(
            string token,
            TokenValidationParameters tokenValidationParameters,
            out SecurityToken securityToken,
            out ClaimsPrincipal claimsPrincipal)
        {
            securityToken = null;
            claimsPrincipal = null;

            if (!JwtSecurityTokenHandler.CanReadToken(token))
            {
                return false;
            }

            var result = exceptionHandler.Try(
                securityToken, 
                (validatedToken) => JwtSecurityTokenHandler
                    .ValidateToken(token, tokenValidationParameters, out validatedToken), (exception) => null,
                exceptionTypes: builder => builder.GetType<SecurityTokenException>());

            claimsPrincipal = result;

            return result == null;
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

        private readonly IExceptionHandler exceptionHandler;
        private readonly Lazy<JwtSecurityTokenHandler> lazyJwtSecurityTokenHandler;
        private JwtSecurityTokenHandler JwtSecurityTokenHandler => lazyJwtSecurityTokenHandler.Value;
    }
}
