using DNI.Core.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class UtilityTests
    {
        [Test]
        public void Copy()
        {
            var nowDate = DateTimeOffset.Now;
            var originalUser = new User
            {
                EmailAddress = "test@domain.com",
                Created = nowDate,
                Id = 1
            };

            var copiedUser = Utilities.Copy(originalUser);

            Assert.AreEqual(originalUser.EmailAddress, copiedUser.EmailAddress);
            Assert.AreEqual(originalUser.Created, copiedUser.Created);
            Assert.AreEqual(originalUser.Id, copiedUser.Id);

            copiedUser.EmailAddress = "test2@domain.com";
            copiedUser.Id = 22;

            Assert.AreNotEqual(originalUser.EmailAddress, copiedUser.EmailAddress);
            Assert.AreEqual(originalUser.Created, copiedUser.Created);
            Assert.AreNotEqual(originalUser.Id, copiedUser.Id);

        }
    }
}
