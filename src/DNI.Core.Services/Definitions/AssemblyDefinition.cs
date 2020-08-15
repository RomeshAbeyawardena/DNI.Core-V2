using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    public class AssemblyDefinition : DefinitionBase<Assembly>, IAssemblyDefinition
    {
        public AssemblyDefinition(IEnumerable<Assembly> assemblies = null)
            : base(assemblies)
        {

        }

        public IEnumerable<Assembly> Assemblies => Definitions;

        public new IAssemblyDefinition Add(Assembly assembly)
        {
            base.Add(assembly);
            return this;
        }

        public IAssemblyDefinition GetAssembly<T>()
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            return Add(assembly);
        }
    }
}
