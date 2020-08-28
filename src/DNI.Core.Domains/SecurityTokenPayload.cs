using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class SecurityTokenPayload : ISecurityTokenPayload
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime? NotBefore { get; set; }
        public DateTime? Expires { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}
