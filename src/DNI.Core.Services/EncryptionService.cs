using DNI.Core.Contracts;
using DNI.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string Decrypt(string encryptedValue, IEncryptionProfile encryptionProfile)
        {
            var value = Convert.FromBase64String(encryptedValue);
            var result = string.Empty;
            InvokeSymmetricAlgorithm(symmetricAlgorithm =>
            {

                var encryptor = symmetricAlgorithm.CreateDecryptor(
                    encryptionProfile.Key.ToArray(),
                    encryptionProfile.InitialVector.ToArray());

                using var memoryStream = new MemoryStream(value);
                using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read);
                using var streamReader = new StreamReader(cryptoStream);
                    result = streamReader.ReadToEnd();
            },
                encryptionProfile.SymmetricAlgorithmName);

            return result;
        }

        public string Encrypt(string value, IEncryptionProfile encryptionProfile)
        {
            byte[] result = null;
            InvokeSymmetricAlgorithm(symmetricAlgorithm =>
            {

                var encryptor = symmetricAlgorithm.CreateEncryptor(
                    encryptionProfile.Key.ToArray(),
                    encryptionProfile.InitialVector.ToArray());

                using var memoryStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                    streamWriter.Write(value);
                 result = memoryStream.ToArray();
            },
                encryptionProfile.SymmetricAlgorithmName);

            return Convert.ToBase64String(result);
        }

        public string Hash(string value, IEncryptionProfile encryptionProfile)
        {
            var byteValue = encryptionProfile.Encoding.GetBytes(value);
            using var hashAlgorithm = HashAlgorithm
                .Create(GetHashAlgorithmName(encryptionProfile.HashAlgorithmType).Name);

            return Convert.ToBase64String(hashAlgorithm.ComputeHash(byteValue));
        }

        private HashAlgorithmName GetHashAlgorithmName(HashAlgorithmType hashAlgorithmType)
        {
            switch (hashAlgorithmType)
            {
                case HashAlgorithmType.Md5:
                    return HashAlgorithmName.MD5;
                case HashAlgorithmType.Sha1:
                    return HashAlgorithmName.SHA1;
                case HashAlgorithmType.Sha256:
                    return HashAlgorithmName.SHA256;
                case HashAlgorithmType.Sha384:
                    return HashAlgorithmName.SHA384;
                case HashAlgorithmType.Sha512:
                    return HashAlgorithmName.SHA512;
                    case HashAlgorithmType.None:
                default:
                    throw new NotSupportedException();
            }
        }

        private void InvokeSymmetricAlgorithm(Action<SymmetricAlgorithm> invokeAction, string symmetricAlgorithmName)
        {
            using (var symmetricAlgorithm = SymmetricAlgorithm.Create(symmetricAlgorithmName))
            {
                invokeAction(symmetricAlgorithm);
            }
        }
    }
}
