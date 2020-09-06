using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNI.Core.Contracts.Services
{
    public interface IJsonTokenService
    {
        string CreateToken(Action<SecurityTokenDescriptor> populateSecurityTokenDescriptor, DateTime expiry,
            IDictionary<string, string> claimsDictionary, string secret, Encoding encoding);
        
        bool TryParseToken(string token, string secret, Action<TokenValidationParameters> populateTokenValidationParameters,
            Encoding encoding, out IDictionary<string, string> claims);
    }
}
