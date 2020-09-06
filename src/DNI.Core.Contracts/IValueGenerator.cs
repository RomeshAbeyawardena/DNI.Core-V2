using System;

namespace DNI.Core.Contracts
{
    public interface IValueGenerator
    {
        Func<object, object> GenerateValue { get; }
        bool ExpectsValue { get; }
    }
}
