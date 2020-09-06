using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.Internal;
using System;

namespace DNI.Core.Services.Implementations.Generators
{
    [IgnoreScanning]
    internal sealed class DateTimeValueGenerator : IValueGenerator
    {
        public DateTimeValueGenerator(ISystemClock systemClock)
        {
            SystemClock = systemClock;
        }

        public Func<object,object> GenerateValue => (value) => SystemClock.UtcNow.UtcDateTime;
        
        public bool ExpectsValue => false;

        private ISystemClock SystemClock { get; }
    }
}
