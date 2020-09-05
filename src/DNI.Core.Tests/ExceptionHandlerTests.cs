using DNI.Core.Services.Handlers;
using DNI.Core.Services.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class ExceptionHandlerTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new ExceptionHandler();
        }

        [Test]
        public void Try()
        {
            Assert.True(
                sut.Try<int, bool>(1, i => i == 1, (ex) => false));

            Assert.False(
                sut.Try<int, bool>(1, i => i < 1, (ex) => false));

            Assert.Throws<NullReferenceException>(
                () => sut.Try(2, i => { if(i != 1) throw new NullReferenceException(); return true; }, ex => false));

            Assert.DoesNotThrow(
                () => sut.Try<int, bool>(2, i => { if(i != 1) throw new NullReferenceException(); return true; }, ex => false, exceptionTypes: t => t.DescribeType<NullReferenceException>()));
        }

        [Test]
        public void TestAsync()
        {
            Assert.ThrowsAsync<NullReferenceException>(
                () => sut.TryAsync(2, async (i) =>
                {
                    if (i != 1)
                        throw new NullReferenceException();
                    return await Task.FromResult(true);
                }, async (ex) => await Task.FromResult(false)));


            Assert.DoesNotThrowAsync(
                async () => await sut.TryAsync<int>(1, 
                async(number) => { 
                    if(number ==1) 
                        throw new FormatException(); 
                    await Task.CompletedTask; },
                (exception) => { return Task.CompletedTask; },
                definitionTypes => definitionTypes.DescribeType<FormatException>()
                ));

            Assert.DoesNotThrowAsync(
                async() => await sut.TryAsync<int, bool>(2, async(i) => { 
                    if(i != 1) 
                        throw new NullReferenceException(); 
                    return await Task.FromResult(true);
                }, async(ex) => await Task.FromResult(false), exceptionTypes: t => t.DescribeType<NullReferenceException>()));

        }

        private ExceptionHandler sut;
    }
}
