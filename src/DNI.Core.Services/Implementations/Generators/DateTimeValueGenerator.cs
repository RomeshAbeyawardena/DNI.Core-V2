using DNI.Core.Contracts;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations.Generators
{
    public sealed class DateTimeValueGenerator : IValueGenerator
    {
        public DateTimeValueGenerator(ISystemClock systemClock)
        {
            SystemClock = systemClock;
        }

        public Func<object> GenerateValue => () => SystemClock.UtcNow.UtcDateTime;

        private ISystemClock SystemClock { get; }
    }
}
