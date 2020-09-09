using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Services.Definitions
{
    [IgnoreScanning]
    public class AssemblyDefinition : DefinitionBase<Assembly>
    {
        private AssemblyDefinition(IEnumerable<Assembly> assemblies = null)
            : base(assemblies)
        {

        }

        public static IDefinition<Assembly> Create(IEnumerable<Assembly> assemblies = null)
        {
            return new AssemblyDefinition(assemblies);
        }

        public static IDefinition<Assembly> Default => new AssemblyDefinition();
    }
}
