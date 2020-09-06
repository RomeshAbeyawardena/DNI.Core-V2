using DNI.Core.Contracts;
using System;

namespace DNI.Core.Tests.Generators
{
    public class TestValueGeneratorA : IValueGenerator
    {
        public Func<object, object> GenerateValue => throw new NotImplementedException();
        public bool ExpectsValue => false;
    }
}
