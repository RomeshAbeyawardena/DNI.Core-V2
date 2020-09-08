using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ICacheStateItem<TState>
    {
        public string Key { get; set; }
        public TState State  { get; set; }
    }
}
