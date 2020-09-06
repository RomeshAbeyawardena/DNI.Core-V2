using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace DNI.Core.Services
{
    public class EncryptionService : IEncryptionService
    {
        public EncryptionService(IGuidService guidService, IMemoryStreamProvider memoryStreamProvider)
        {
            this.guidService = guidService;
            this.memoryStreamProvider = memoryStreamProvider;
        }

        public string Decrypt(string encryptedValue, IEncryptionProfile encryptionProfile)
        {
            var value = Convert.FromBase64String(encryptedValue);
            var result = string.Empty;
            InvokeSymmetricAlgorithm(symmetricAlgorithm =>
            {

                var encryptor = symmetricAlgorithm.CreateDecryptor(
                    encryptionProfile.Key.ToArray(),
                    encryptionProfile.InitialVector.ToArray());

                using var memoryStream =  memoryStreamProvider.GetMemoryStream(guidService.GenerateGuid(), "decrypt", value);
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

                using var memoryStream = memoryStreamProvider.GetMemoryStream(guidService.GenerateGuid(), "encrypt");
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

            return Convert.ToBase64String (
                hashAlgorithm.ComputeHash (
                    SaltKey(encryptionProfile.Salt, byteValue).ToArray()));
        }

        public IEnumerable<byte> SaltKey(IEnumerable<byte> salt, IEnumerable<byte> key)
        {
            if(salt == null || salt.IsEmpty())
            {
                throw new ArgumentNullException(nameof(salt));
            }

            if(key == null || key.IsEmpty())
            {
                throw new ArgumentNullException(nameof(key));
            }

            var saltedKey = new List<byte>();

            saltedKey.AddRange(salt.ToArray());
            saltedKey.AddRange(key.ToArray());

            return saltedKey.ToArray();
        }

        public IEnumerable<byte> SaltKey(IEnumerable<byte> key, out IEnumerable<byte> salt)
        {
            salt = guidService.GenerateGuid().ToByteArray();
            return SaltKey(salt, key);
        }

        private HashAlgorithmName GetHashAlgorithmName(HashAlgorithmType hashAlgorithmType)
        {
            return hashAlgorithmType switch
            {
                HashAlgorithmType.Md5 => HashAlgorithmName.MD5,
                HashAlgorithmType.Sha1 => HashAlgorithmName.SHA1,
                HashAlgorithmType.Sha256 => HashAlgorithmName.SHA256,
                HashAlgorithmType.Sha384 => HashAlgorithmName.SHA384,
                HashAlgorithmType.Sha512 => HashAlgorithmName.SHA512,
                _ => throw new NotSupportedException("Specified hash algorithm not supported"),
            };
        }

        private void InvokeSymmetricAlgorithm(Action<SymmetricAlgorithm> invokeAction, string symmetricAlgorithmName)
        {
            using var symmetricAlgorithm = SymmetricAlgorithm.Create(symmetricAlgorithmName);
            invokeAction(symmetricAlgorithm);
            
        }

        private readonly IGuidService guidService;
        private readonly IMemoryStreamProvider memoryStreamProvider;
    }
}
