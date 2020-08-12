using DNI.Core.Contracts;
using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    public class ValueGeneratorProvider : IValueGeneratorProvider
    {
        public ValueGeneratorProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            valueGeneratorFactory = serviceProvider.GetRequiredService<IValueGeneratorFactory>();
        }

        public IValueGenerator GetValueGeneratorByName(string generatorName, bool usesDefaultServiceInjector = true)
        {
            if (usesDefaultServiceInjector && valueGeneratorFactory.TryGetValue(generatorName, out var generatorType))
            {
                return (IValueGenerator)serviceProvider.GetService(generatorType);
            }

            throw new NotSupportedException();
        }
        
        private readonly IValueGeneratorFactory valueGeneratorFactory;
        private readonly IServiceProvider serviceProvider;
    }
}
