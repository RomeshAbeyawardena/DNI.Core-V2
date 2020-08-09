using DNI.Core.Contracts;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations.Generators
{
    public sealed class DateTimeOffSetValueGenerator : IValueGenerator
    {
        public DateTimeOffSetValueGenerator(ISystemClock systemClock)
        {
            SystemClock = systemClock;
        }

        public Func<object, object> GenerateValue => (value) => SystemClock.UtcNow;
        
        private ISystemClock SystemClock { get; }
    }
}
