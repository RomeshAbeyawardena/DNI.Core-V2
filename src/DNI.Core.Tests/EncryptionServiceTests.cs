using DNI.Core.Contracts;
using DNI.Core.Contracts.Services;
using DNI.Core.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class EncryptionServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            encryptionProfileMock = new Mock<IEncryptionProfile>();
            guidServiceMock = new Mock<IGuidService>();
            sut = new EncryptionService(guidServiceMock.Object);
        }

        [Test]
        public void Encrypt_Decrypt()
        {
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

        private Mock<IGuidService> guidServiceMock;
        private Mock<IEncryptionProfile> encryptionProfileMock;
        private EncryptionService sut;
    }
}
