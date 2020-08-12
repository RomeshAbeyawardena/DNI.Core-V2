using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IEncryptionProfile
    {
        IEnumerable<byte> Key { get; }
        IEnumerable<byte> InitialVector { get; }
        IEnumerable<byte> Salt { get; }
        HashAlgorithmType HashAlgorithmType { get; }
        string SymmetricAlgorithmName { get; }
        Encoding Encoding { get; }
    }
}
