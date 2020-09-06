using DNI.Core.Contracts;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace DNI.Core.Domains
{
    public class EncryptionProfile : IEncryptionProfile
    {
        public IEnumerable<byte> Key { get; set; }
        public IEnumerable<byte> InitialVector { get; set; }
        public IEnumerable<byte> Salt { get; set; }
        public HashAlgorithmType HashAlgorithmType { get; set; }
        public string SymmetricAlgorithmName { get; set; }
        public Encoding Encoding { get; set; }

    }
}
