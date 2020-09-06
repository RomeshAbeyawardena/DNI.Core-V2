using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.Internal;
using System;

namespace DNI.Core.Services.Implementations.Generators
{
    [IgnoreScanning]
    internal sealed class DateTimeOffSetValueGenerator : IValueGenerator
    {
        public DateTimeOffSetValueGenerator(ISystemClock systemClock)
        {
            SystemClock = systemClock;
        }

        public Func<object, object> GenerateValue => (value) => SystemClock.UtcNow;

        public bool ExpectsValue => false;

        private ISystemClock SystemClock { get; }
    }
}
