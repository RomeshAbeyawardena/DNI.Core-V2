using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Tests.Generators
{
    public class TestValueGeneratorA : IValueGenerator
    {
        public Func<object, object> GenerateValue => throw new NotImplementedException();
    }
}
