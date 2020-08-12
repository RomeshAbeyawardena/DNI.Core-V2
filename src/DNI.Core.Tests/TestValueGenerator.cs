using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests
{
    public class TestValueGenerator : IValueGenerator
    {
        public TestValueGenerator(object value)
        {
            this.value = value;
        }

        public Func<object, object> GenerateValue => (a) => value;

        private readonly object value;
    }
}
