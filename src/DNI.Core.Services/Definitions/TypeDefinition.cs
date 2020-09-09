using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;

namespace DNI.Core.Services.Definitions
{
    [IgnoreScanning]
    public class TypeDefinition : DefinitionBase<Type>
    {
        private TypeDefinition(IEnumerable<Type> types = null)
            : base(types)
        {
            
        }

        public static IDefinition<Type> Create(IEnumerable<Type> types = null)
        {
            return new TypeDefinition(types);
        }

        public static IDefinition<Type> Default => TypeDefinition.Create();
    }
}
