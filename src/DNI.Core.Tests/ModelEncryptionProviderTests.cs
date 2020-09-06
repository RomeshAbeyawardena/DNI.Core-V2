using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Providers;
using Moq;
using NUnit.Framework;
using System;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Enumerations;

namespace DNI.Core.Tests
{
    public class UserDto
    {
        public int Id { get; set; }

        [Encrypt(EncryptionMethod.TwoWay, EncryptionClassification.Personal)]
        public string EmailAddress { get; set; }

        public DateTimeOffset Created { get; set; }
    }

    [TestFixture]
    public class ModelEncryptionProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            encryptionServiceMock = new Mock<IEncryptionService>();
            mapperProviderMock = new Mock<IMapperProvider>();
            encryptionProfileManagerMock = new Mock<IEncryptionProfileManager>();
            sut = new ModelEncryptionProvider(
                encryptionServiceMock.Object,
                mapperProviderMock.Object,
                encryptionProfileManagerMock.Object);
        }

        [Test]
        public void Decrypt()
        {
            mapperProviderMock.Setup(mapper => mapper.Map<User, UserDto>(It.IsAny<User>()))
                .Returns(new UserDto { EmailAddress = "encrypted-test@gmail.com" });

            encryptionProfileManagerMock.Setup(epm => epm
                .TryGetValue(EncryptionClassification.Personal, out It.Ref<IEncryptionProfile>.IsAny))
                .Returns(true);

            encryptionServiceMock.Setup(encryptionService => encryptionService.Decrypt("test@gmail.com", It.IsAny<IEncryptionProfile>()))
                .Returns("decrypted-test@gmail.com");

            var originalUser = new User { EmailAddress = "test@gmail.com" };
            var decrypted = sut.Decrypt<User, UserDto>(originalUser);

            Assert.AreEqual("decrypted-test@gmail.com", decrypted.EmailAddress);
        }

        [Test]
        public void Encrypt()
        {
            mapperProviderMock.Setup(mapper => mapper.Map<User, UserDto>(It.IsAny<User>()))
                .Returns(new UserDto { EmailAddress = "encrypted-test@gmail.com" });

            encryptionProfileManagerMock.Setup(epm => epm
                .TryGetValue(EncryptionClassification.Personal, out It.Ref<IEncryptionProfile>.IsAny))
                .Returns(true);

            encryptionServiceMock.Setup(encryptionService => encryptionService.Encrypt("test@gmail.com", It.IsAny<IEncryptionProfile>()))
                .Returns("decrypted-test@gmail.com");

            var originalUser = new User { EmailAddress = "test@gmail.com" };
            var decrypted = sut.Encrypt<User, UserDto>(originalUser);

            Assert.AreEqual("decrypted-test@gmail.com", decrypted.EmailAddress);
        }

        private Mock<IEncryptionService> encryptionServiceMock;
        private Mock<IMapperProvider> mapperProviderMock;
        private Mock<IEncryptionProfileManager> encryptionProfileManagerMock;
        private ModelEncryptionProvider sut;
    }
}
