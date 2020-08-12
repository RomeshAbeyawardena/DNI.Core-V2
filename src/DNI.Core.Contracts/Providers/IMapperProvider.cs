using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IMapperProvider
    {
        TDestination Map<TDestination>(object source);
        IEnumerable<TDestination> Map<TDestination>(IEnumerable<object> sources);
    }
}
