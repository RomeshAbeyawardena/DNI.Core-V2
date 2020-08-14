using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IEncryptionProfile
    {
        IEnumerable<byte> Key { get; set; }
        IEnumerable<byte> InitialVector { get; set; }
        IEnumerable<byte> Salt { get; set; }
        HashAlgorithmType HashAlgorithmType { get; set; }
        string SymmetricAlgorithmName { get; set; }
        Encoding Encoding { get; set; }
    }
}
