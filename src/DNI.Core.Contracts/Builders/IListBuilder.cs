using System.Collections.Generic;

namespace DNI.Core.Contracts.Builders
{
    public interface IListBuilder<T> : IEnumerable<T>
    {
        IListBuilder<T> Add(T item);
        IListBuilder<T> AddRange(IEnumerable<T> items);
    }
}
