using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    public class TypeDefinition : DefinitionBase<Type>, IDefinition<Type>
    {
        public TypeDefinition(IEnumerable<Type> types = null)
            : base(types)
        {
            
        }

    }
}
