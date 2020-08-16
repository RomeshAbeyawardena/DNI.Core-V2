using DNI.Core.Contracts.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Builders
{
    public class ListBuilder<T> : IListBuilder<T>
    {
        public ListBuilder(IEnumerable<T> items = null)
        {
            if(items == null)
            {
                list = new List<T>();
            }
            else
            {
                list = new List<T>(items);
            }
        }

        public IListBuilder<T> Add(T item)
        {
             list.Add(item);
            return this;
        }

        public IListBuilder<T> AddRange(IEnumerable<T> items)
        {
            list.AddRange(items);
            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        private readonly List<T> list;
    }
}
