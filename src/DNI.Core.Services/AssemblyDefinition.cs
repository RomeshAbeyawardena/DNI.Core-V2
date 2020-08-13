using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal class AssemblyDefinition : IAssemblyDefinition
    {
        public AssemblyDefinition(IEnumerable<Assembly> assemblies = default)
        {
            this.assemblies = new List<Assembly>(assemblies);
        }

        public IEnumerable<Assembly> Assemblies => assemblies.ToArray();

        public IAssemblyDefinition GetAssembly<T>()
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            if (!assemblies.Contains(assembly))
            {
                assemblies.Add(assembly);
            }

            return this;
        }

        private readonly List<Assembly> assemblies;
    }
}
