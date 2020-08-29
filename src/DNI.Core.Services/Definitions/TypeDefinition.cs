using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    [IgnoreScanning]
    public class TypeDefinition : DefinitionBase<Type>
    {
        public TypeDefinition(IEnumerable<Type> types = null)
            : base(types)
        {
            
        }

    }
}
