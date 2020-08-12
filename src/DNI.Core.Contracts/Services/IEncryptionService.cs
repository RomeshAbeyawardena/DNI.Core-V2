using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string value, IEncryptionProfile encryptionProfile);
        string Decrypt(string encryptedValue, IEncryptionProfile encryptionProfile);
        string Hash(string value, IEncryptionProfile encryptionProfile);
    }
}
