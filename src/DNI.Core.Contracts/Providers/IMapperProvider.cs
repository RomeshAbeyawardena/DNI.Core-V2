using System.Collections.Generic;

namespace DNI.Core.Contracts.Providers
{
    public interface IMapperProvider
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TDestination>(object source);
        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sources);
        IEnumerable<TDestination> Map<TDestination>(IEnumerable<object> sources);
    }
}
