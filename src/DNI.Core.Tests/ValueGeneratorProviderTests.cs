﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Core.Services.Providers;
using DNI.Core.Contracts.Managers;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using NUnit.Framework;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class ValueGeneratorProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            valueGeneratorManagerMock = new Mock<IValueGeneratorManager>();
            sut = new ValueGeneratorProvider(serviceProviderMock.Object, valueGeneratorManagerMock.Object);
        }

        [Test]
        public void GetValueGeneratorByName()
        {
            const bool expectedValue = true;

            valueGeneratorManagerMock.Setup(valueGeneratorManager => valueGeneratorManager
                .TryGetValue("myValueGenerator", out It.Ref<Type>.IsAny))
                .Returns(true)
                .Verifiable();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(It.IsAny<Type>()))
                .Returns(new TestValueGenerator(expectedValue))
                .Verifiable();

            var generator = sut.GetValueGeneratorByName("myValueGenerator");
            valueGeneratorManagerMock.Verify();

            serviceProviderMock.Verify();

            Assert.IsNotNull(generator);
            Assert.AreEqual(expectedValue, generator.GenerateValue(false));
        }

        private Mock<IServiceProvider> serviceProviderMock;
        private Mock<IValueGeneratorManager> valueGeneratorManagerMock;
        private ValueGeneratorProvider sut;
    }
}
