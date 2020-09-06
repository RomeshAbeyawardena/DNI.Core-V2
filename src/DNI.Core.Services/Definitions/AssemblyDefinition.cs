using DNI.Core.Shared.Attributes;
using System.Collections.Generic;
using System.Reflection;

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
