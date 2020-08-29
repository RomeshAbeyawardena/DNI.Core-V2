using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    public class AssemblyDefinition : DefinitionBase<Assembly>, IDefinition<Assembly>
    {
        public AssemblyDefinition(IEnumerable<Assembly> assemblies = null)
            : base(assemblies)
        {

        }

    }
}
