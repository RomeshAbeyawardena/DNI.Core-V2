using System.Collections.Generic;

namespace DNI.Core.Contracts.Services
{
    public interface IEncryptionService
    {
        IEnumerable<byte> SaltKey(IEnumerable<byte> salt, IEnumerable<byte> key);
        IEnumerable<byte> SaltKey(IEnumerable<byte> key, out IEnumerable<byte> salt);
        string Encrypt(string value, IEncryptionProfile encryptionProfile);
        string Decrypt(string encryptedValue, IEncryptionProfile encryptionProfile);
        string Hash(string value, IEncryptionProfile encryptionProfile);
    }
}
