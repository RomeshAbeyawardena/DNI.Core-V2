using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class EncryptionServiceTests
    {
        delegate void Asm(Guid s, string m, IEnumerable<byte> val);

        [SetUp]
        public void SetUp()
        {
            encryptionProfileMock = new Mock<IEncryptionProfile>();
            memoryStreamProviderMock = new Mock<IMemoryStreamProvider>();
            guidServiceMock = new Mock<IGuidService>();
            sut = new EncryptionService(guidServiceMock.Object, memoryStreamProviderMock.Object);
        }

        [Test]
        public void Encrypt_Decrypt()
        {
            byte[] byteValue = Array.Empty<byte>();
            memoryStreamProviderMock.Setup(memoryStreamProvider => memoryStreamProvider
                .GetMemoryStream(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback(new Asm((a, b, c) => { byteValue = c.ToArray(); }))
                    .Returns(() => new MemoryStream(byteValue));

            memoryStreamProviderMock.Setup(memoryStreamProvider => memoryStreamProvider
                .GetMemoryStream(It.IsAny<Guid>(), It.IsAny<string>()))
                    .Returns(new MemoryStream());


            const string expectedValue = "Lorem Ipsum";
            encryptionProfileMock
                .SetupGet(encryptionProfile => encryptionProfile.SymmetricAlgorithmName)
                .Returns(nameof(Aes));

            var salt = Guid.NewGuid();
            var key = Guid.NewGuid();

            var saltedKey = new List<byte>();

            saltedKey.AddRange(salt.ToByteArray());
            saltedKey.AddRange(key.ToByteArray());

            var initialVector = Guid.NewGuid();

            encryptionProfileMock
                .SetupGet(encryptionProfile => encryptionProfile.InitialVector)
                .Returns( initialVector.ToByteArray());

            encryptionProfileMock
                .SetupGet(encryptionProfile => encryptionProfile.Key)
                .Returns(saltedKey.ToArray());


            var encryptedValue = sut.Encrypt(expectedValue, encryptionProfileMock.Object);
            var value = sut.Decrypt(encryptedValue, encryptionProfileMock.Object);

            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void SaltKey()
        {
            var expectedGuid = Guid.NewGuid();

            var salt = Guid.NewGuid().ToByteArray();
            var key = Guid.NewGuid().ToByteArray();
            guidServiceMock.Setup(guidService => guidService.GenerateGuid())
                .Returns(expectedGuid);

            var result = sut.SaltKey(salt, key);

            Assert.AreEqual(key.Length + salt.Length, result.Count());

            result = sut.SaltKey(key, out var newSalt);
            Assert.AreEqual(expectedGuid.ToByteArray(), newSalt);
        }

        [Test]
        public void Hash()
        {
            const string originalValue = "My$3cureP@s$w0rd!";
            encryptionProfileMock
                .SetupGet(encryptionProfile => encryptionProfile.Encoding)
                .Returns(Encoding.ASCII);

            encryptionProfileMock
                .SetupGet(encryptionProfile => encryptionProfile.HashAlgorithmType)
                .Returns(HashAlgorithmType.Sha512);

            encryptionProfileMock
                .SetupGet(encryptionProfile => encryptionProfile.Salt)
                .Returns(Guid.NewGuid().ToByteArray());


            var hashedValue = sut.Hash(originalValue, encryptionProfileMock.Object);

            Assert.AreNotEqual(originalValue, hashedValue);

            var repeatedHashedValue = sut.Hash(originalValue, encryptionProfileMock.Object);

            Assert.AreEqual(hashedValue, repeatedHashedValue);
        }

        private Mock<IMemoryStreamProvider> memoryStreamProviderMock;
        private Mock<IGuidService> guidServiceMock;
        private Mock<IEncryptionProfile> encryptionProfileMock;
        private EncryptionService sut;
    }
}
