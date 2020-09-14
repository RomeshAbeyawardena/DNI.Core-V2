using DNI.Core.Services.Providers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{

    [TestFixture]
    public class JsonStreamSerializerProviderTests
    {

        [SetUp]
        public void SetUp()
        {
            //SetUp code here
            sut = new JsonStreamSerializerProvider(JsonSerializer.CreateDefault(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        [TearDown]
        public void TearDown()
        {
            sut.Dispose();
        }

        [Test]
        public async Task SerializeAsync()
        {
            //Test code here
            var expectedType = typeof(User);
            using var stream = new MemoryStream();
            await sut.SerializeStreamAsync(stream, expectedType, CancellationToken.None);
            stream.Position = 0;
            var newType = await sut.DeserializeStreamAsync<Type>(stream, CancellationToken.None);
            
            Assert.AreEqual(expectedType, newType);

            await Task.CompletedTask;
        }

        [Test]
        public async Task SerializeListAsync()
        {
            //Test code here
            var expectedType = new List<Type> { typeof(User), typeof(JsonStreamSerializerProvider) };
            using var stream = new MemoryStream();
            await sut.SerializeStreamAsync(stream, expectedType.ToArray(), CancellationToken.None);
            stream.Position = 0;
            var newType = await sut.DeserializeStreamAsync<IEnumerable<Type>>(stream, CancellationToken.None);
            
            Assert.AreEqual(expectedType, newType);
            Assert.Contains(typeof(User), newType.ToArray());
            Assert.Contains(typeof(JsonStreamSerializerProvider), newType.ToArray());
            await Task.CompletedTask;
        }



        private JsonStreamSerializerProvider sut;
    }
}
