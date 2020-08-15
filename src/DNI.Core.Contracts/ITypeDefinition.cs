using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ITypeDefinition : IDefinition<Type>
    {
        new ITypeDefinition Add(Type type);
        ITypeDefinition GetType<TType>();
        IEnumerable<Type> Types { get; }
    }
}
