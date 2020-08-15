using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IAssemblyDefinition : IDefinition<Assembly>
    {
        new IAssemblyDefinition Add(Assembly assembly);
        IAssemblyDefinition GetAssembly<T>();
        IEnumerable<Assembly> Assemblies { get; }
    }

}
