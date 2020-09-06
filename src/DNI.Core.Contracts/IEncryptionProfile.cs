using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

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
