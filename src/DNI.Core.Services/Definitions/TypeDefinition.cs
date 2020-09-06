using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;

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
