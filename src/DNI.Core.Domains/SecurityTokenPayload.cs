using DNI.Core.Contracts;
using System;
using System.Collections.Generic;

namespace DNI.Core.Domains
{
    [MessagePack.MessagePackObject(true)]
    public class SecurityTokenPayload : ISecurityTokenPayload
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime? NotBefore { get; set; }
        public DateTime? Expires { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}
