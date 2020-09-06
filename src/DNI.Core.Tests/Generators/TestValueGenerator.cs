using DNI.Core.Contracts;
using System;

namespace DNI.Core.Tests.Generators
{
    public class TestValueGenerator : IValueGenerator
    {
        public TestValueGenerator(object value)
        {
            this.value = value;
        }

        public Func<object, object> GenerateValue => (a) => value;
        public bool ExpectsValue => false;
        private readonly object value;
    }
}
