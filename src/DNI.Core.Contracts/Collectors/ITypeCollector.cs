using System;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Contracts.Collectors
{
    public interface ITypeCollector : ICollector<Type>
    {
        IEnumerable<Type> Collect(Type serviceType, IEnumerable<Assembly> assemblies);
        IEnumerable<Type> Collect<TService>(IEnumerable<Type> types);
        IEnumerable<Type> Collect<TService>(Action<IDefinition<Type>> describeTypes);
        IEnumerable<Type> Collect<TService>(IEnumerable<Assembly> assemblies);
        IEnumerable<Type> Collect<TService>(Action<IDefinition<Assembly>> describeAssemblies);
    }
}
