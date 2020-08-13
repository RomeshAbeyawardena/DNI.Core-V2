using AutoMapper;
using DNI.Core.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    internal class AutoMapperProvider : IMapperProvider
    {
        private readonly IMapper mapper;

        public AutoMapperProvider(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }

        public IEnumerable<TDestination> Map<TDestination>(IEnumerable<object> sources)
        {
            return mapper.Map<IEnumerable<TDestination>>(sources);
        }
    }
}
