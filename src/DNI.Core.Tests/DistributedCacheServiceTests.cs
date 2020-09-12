using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Handlers;
using DNI.Core.Services.Implementations.Cache;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.Json;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class DistributedCacheServiceTests
    {
        private DistributedCacheEntryOptions DistributedCacheEntryOptions => new DistributedCacheEntryOptions { 
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) 
                };

        [SetUp]
        public void SetUp()
        {
            memoryStreamProviderMock = new Mock<IMemoryStreamProvider>();
            distributedCacheMock = new Mock<IDistributedCache>();
            distributedCacheService = new DistributedCacheService(memoryStreamProviderMock.Object,
                new ExceptionHandler(), distributedCacheMock.Object, DistributedCacheEntryOptions, 
                new JsonSerializerOptions(), MessagePackSerializerOptions.Standard);
        }

        [Test]
        public async Task GetAsync()
        {
            var expectedUser = new User { EmailAddress = "info@kmi.net", Id = 1, Surname = "Pototo2" };
            var  result = MessagePackSerializer.Serialize(expectedUser, MessagePackSerializerOptions.Standard);
            
            distributedCacheMock.Setup(distributedCache => distributedCache.GetAsync("key", CancellationToken.None))
                .Returns(Task.FromResult(result));

            memoryStreamProviderMock.Setup(memoryStreamProvider => memoryStreamProvider.GetMemoryStream(It.IsAny<IEnumerable<byte>>()))
                .Returns(new System.IO.MemoryStream(result));

            var attempt = await distributedCacheService.GetAsync<User>("key", CancellationToken.None);

            Assert.True(attempt.Successful);

            Assert.IsNotNull(attempt.Result);

            Assert.AreEqual(expectedUser, attempt.Result);
        }


        [Test]
        public async Task SetAsync()
        {
            var expectedUser = new User { EmailAddress = "info@kmi.net", Id = 1, Surname = "Pototo2" };
            var  result = MessagePackSerializer.Serialize(expectedUser, MessagePackSerializerOptions.Standard);
            
            distributedCacheMock.Setup(distributedCache => distributedCache.SetAsync("key", result, DistributedCacheEntryOptions, CancellationToken.None))
                .Returns(Task.FromResult(result));

            memoryStreamProviderMock.Setup(memoryStreamProvider => memoryStreamProvider.GetMemoryStream())
                .Returns(new System.IO.MemoryStream());

            var attempt = await distributedCacheService.SetAsync("key", expectedUser, CancellationToken.None);

            Assert.True(attempt.Successful);

            Assert.IsNotNull(attempt.Result);

            Assert.AreEqual(expectedUser, attempt.Result);
        }

        [MessagePackObject(true)]
        public class User
        {
            public int Id { get; set; }
            public string EmailAddress { get; set; }
            public string Surname { get; set; }

            public override bool Equals(object obj)
            {
                if(obj is User user)
                {
                    return Equals(user);
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, EmailAddress, Surname);
            }

            private bool Equals(User user)
            {
                return user.EmailAddress.Equals(EmailAddress)
                    && user.Id == Id
                    && user.Surname.Equals(Surname);
            }
        }

        private Mock<IMemoryStreamProvider> memoryStreamProviderMock;
        private Mock<IDistributedCache> distributedCacheMock;
        private DistributedCacheService distributedCacheService;
    }
}
