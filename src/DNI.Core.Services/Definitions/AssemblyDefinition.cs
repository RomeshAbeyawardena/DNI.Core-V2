using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    [IgnoreScanning]
    public class AssemblyDefinition : DefinitionBase<Assembly>
    {
        public AssemblyDefinition(IEnumerable<Assembly> assemblies = null)
            : base(assemblies)
        {

        }

    }
}
