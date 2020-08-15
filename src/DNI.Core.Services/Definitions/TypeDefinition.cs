using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    public class TypeDefinition : DefinitionBase<Type>, ITypeDefinition
    {
        public TypeDefinition(IEnumerable<Type> types = null)
            : base(types)
        {
            
        }

        public IEnumerable<Type> Types => Definitions;

        public new ITypeDefinition Add(Type type)
        {
            base.Add(type);
            return this;
        }

        public ITypeDefinition GetType<TType>()
        {
            var type = typeof(TType);
            return Add(type);
        }
    }
}
