using System;
using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface ISecurityTokenPayload
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        DateTime? NotBefore { get; set; }
        DateTime? Expires { get; set; }
        IEnumerable<string> Claims { get; set; }
    }
}
