using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
