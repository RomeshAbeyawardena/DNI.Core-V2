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
        public AssemblyDefinition(IEnumerable<Assembly> assemblies = null)
        {
            if(assemblies == null)
            {
                this.assemblies = new List<Assembly>();
            }
            else
            { 
                this.assemblies = new List<Assembly>(assemblies);
            }
        }

        public IEnumerable<Assembly> Assemblies => assemblies.ToArray();

        public IAssemblyDefinition Add(Assembly assembly)
        {
            if (!assemblies.Contains(assembly))
            {
                assemblies.Add(assembly);
            }

            return this;
        }

        public IAssemblyDefinition GetAssembly<T>()
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            return Add(assembly);
        }

        private readonly List<Assembly> assemblies;
    }
}
