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
        Func<Type, bool> ServiceFilter { get; }

        IEnumerable<Type> Collect(Type serviceType, IEnumerable<Type> serviceTypes);
        IEnumerable<Type> Collect(Type serviceType, IEnumerable<Assembly> assemblies);
        IEnumerable<Type> Collect<TService>(IEnumerable<Type> types);
        IEnumerable<Type> Collect<TService>(Action<IDefinition<Type>> describeTypes);
        IEnumerable<Type> Collect<TService>(IEnumerable<Assembly> assemblies);
        IEnumerable<Type> Collect<TService>(Action<IDefinition<Assembly>> describeAssemblies);
    }
}
