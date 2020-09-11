using System;

namespace DNI.Core.Contracts
{
    public interface IDateRange<TDate> : IComparable<IDateRange<TDate>>
        where TDate : struct
    {
        TDate? Start { get; }
        TDate? End { get; }
    }
}
