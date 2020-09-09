using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IServiceCollector
    {
        IEnumerable<Type> Collect(Type serviceType, params Type[] serviceTypes);
        IEnumerable<Type> Collect(Type serviceType, params Assembly[] assemblies);
        IEnumerable<Type> Collect<TService>(params Assembly[] assemblies);
        IEnumerable<Type> Collect<TService>(Action<IDefinition<Assembly>> describeAssemblies);
    }
}
