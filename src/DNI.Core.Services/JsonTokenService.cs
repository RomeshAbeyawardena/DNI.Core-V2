using DNI.Core.Contracts.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class JsonTokenService : IJsonTokenService
    {
        private SecurityTokenDescriptor GetSecurityTokenDescriptor(Action<SecurityTokenDescriptor> populateSecurityTokenDescriptor,
            SigningCredentials signingCredentials,
            DateTime expiry, IDictionary<string, string> claimsDictionary)
        {
            var claims = claimsDictionary.Select((keyValuePair) => new Claim(keyValuePair.Key, keyValuePair.Value));
            return GetSecurityTokenDescriptor(populateSecurityTokenDescriptor, signingCredentials, expiry, new ClaimsIdentity(claims));
        }

        public static SecurityTokenDescriptor GetSecurityTokenDescriptor(Action<SecurityTokenDescriptor> populateSecurityTokenDescriptor,
            SigningCredentials signingCredentials,
            DateTime expiry, ClaimsIdentity claimsIdentity, DateTime? issuedAt = null)
        {
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expiry,
                SigningCredentials = signingCredentials,
                IssuedAt = issuedAt.HasValue ? issuedAt : DateTime.Now
            };

            populateSecurityTokenDescriptor?.Invoke(securityTokenDescriptor);

            return securityTokenDescriptor;
        }

        private SigningCredentials GetSigningCredentials(string secret, string securityAlgorithm, Encoding encoding)
        {
            var securityKey = new SymmetricSecurityKey(encoding.GetBytes(secret));
            var signinCredentials = new SigningCredentials(securityKey, securityAlgorithm, SecurityAlgorithms.Sha512Digest);

            return signinCredentials;
        }

        public string CreateToken(Action<SecurityTokenDescriptor> populateSecurityTokenDescriptor, DateTime expiry,
            IDictionary<string, string> claimsDictionary, string secret, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);
            var token = tokenHandler.CreateToken(GetSecurityTokenDescriptor(populateSecurityTokenDescriptor, signingCredentials, expiry, claimsDictionary));
            return tokenHandler.WriteToken(token);
        }

        public bool TryParseToken(string token, string secret, Action<TokenValidationParameters> populateTokenValidationParameters,
            Encoding encoding, out IDictionary<string, string> claims)
        {
            var handledExceptions = new[] {
                typeof(SecurityTokenInvalidAudienceException),
                typeof(SecurityTokenInvalidSigningKeyException),
                typeof(SecurityTokenInvalidSignatureException)
            };

            try
            {
                if (encoding == null)
                    throw new ArgumentNullException(nameof(encoding));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters();
                var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);

                populateTokenValidationParameters?.Invoke(tokenValidationParameters);
                tokenValidationParameters.IssuerSigningKey = signingCredentials.Key;

                var securityClaimPrinciple = tokenHandler.ValidateToken(token, tokenValidationParameters, out var m);

                var securityToken = tokenHandler.ReadJwtToken(token);
                claims = securityToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                return true;
            }
            catch (Exception ex)
            {
                if (!handledExceptions.Contains(ex.GetType()))
                    throw;

                claims = null;
                return false;
            }
        }
    }

}
