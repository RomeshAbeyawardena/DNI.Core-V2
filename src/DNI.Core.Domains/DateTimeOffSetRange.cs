using DNI.Core.Contracts;
using System;

namespace DNI.Core.Domains
{
    public sealed class DateTimeOffsetRange : IDateRange<DateTimeOffset>
    {
        public DateTimeOffsetRange(DateTimeOffset start)
            : this(start, null)
        {

        }
        public DateTimeOffsetRange(DateTimeOffset? start, DateTimeOffset? end)
        {
            Start = start;
            End = end;
        }
        public DateTimeOffset? Start { get; }
        public DateTimeOffset? End { get; }

        public override bool Equals(object obj)
        {
            if(!(obj is DateTimeOffsetRange dateTimeOffsetRange))
            {
                return false;
            }

            return Equals(dateTimeOffsetRange);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public int CompareTo(IDateRange<DateTimeOffset> other)
        {
            var compareIndex = 0;

            if (Start.HasValue && other.Start.HasValue)
            {
                compareIndex += Start.Value.CompareTo(other.Start.Value);
            }

            if (End.HasValue && other.End.HasValue)
            {
                compareIndex += Start.Value.CompareTo(other.Start.Value);
            }

            return compareIndex;
        }

        private bool Equals(IDateRange<DateTime> dateRange)
        {
            bool isStartSame = false;
            bool isEndSame = false;
            if(dateRange.Start.HasValue != Start.HasValue
                || dateRange.End.HasValue != dateRange.End.HasValue)
            {
                return false;
            }

            if(!dateRange.Start.HasValue && !Start.HasValue 
                || dateRange.Start.Equals(Start.Value))
            {
                isStartSame = true;
            }

            if(!dateRange.End.HasValue && !End.HasValue 
                || dateRange.End.Equals(End.Value))
            {
                isEndSame = true;
            }

            return isEndSame && isStartSame;
        }
    }
}
