using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services;
using DNI.Core.Services.Builders;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class JwtServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            securityTokenValidatorMock = new Mock<ISecurityTokenValidator>();
            tokenHandlerProviderMock = new Mock<ITokenHandlerProvider>();
            exceptionHandlerMock = new Mock<IExceptionHandler>();
            encryptionProfileMock = new Mock<IEncryptionProfile>();

            tokenHandlerProviderMock.Setup(tokenHandlerProvider => tokenHandlerProvider.GetSecurityTokenHandler())
                .Returns(new JwtSecurityTokenHandler());

            sut = new JsonTokenService();
        }

        [Test]
        public void GenerateToken()
        {

        }

        [Test]
        public void TryReadToken()
        {
            encryptionProfileMock.SetupGet(encryptionProfile => encryptionProfile.Key)
                .Returns(Guid.NewGuid().ToByteArray());

            var token = sut.CreateToken(parameters => { parameters.Issuer = "localhost"; parameters.Audience = "localhost"; },
                    DateTime.Now.AddMinutes(5), DictionaryBuilder.Create<string, string>(builder => builder.Add("UserId", "1"))
                        .Dictionary, "ph73j53ekzzeajqf5b7nycchulo2wkrqevqys6v2lygc5cjlxaia", Encoding.ASCII);

            //exceptionHandlerMock.Verify();

            Assert.True(sut.TryParseToken(token, "ph73j53ekzzeajqf5b7nycchulo2wkrqevqys6v2lygc5cjlxaia", parameters => {
                    parameters.RequireExpirationTime = true;
                    parameters.ValidIssuer = "localhost"; //TODO Find a smart way to manage this data
                    parameters.ValidAudience = "localhost"; //DITTO
                }, Encoding.ASCII, out var claims));
            
            Assert.IsNotNull(claims);
            Assert.IsTrue(claims.TryGetValue("UserId", out var userId));
            Assert.AreEqual("1", userId);
        }

        private Mock<ISecurityTokenValidator> securityTokenValidatorMock;
        private Mock<IEncryptionProfile> encryptionProfileMock;
        private Mock<ITokenHandlerProvider> tokenHandlerProviderMock;
        private Mock<IExceptionHandler> exceptionHandlerMock;
        private JsonTokenService sut;
    }
}
