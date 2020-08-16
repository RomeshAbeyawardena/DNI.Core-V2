using AutoMapper.Internal;
using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

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
