using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    public sealed class ValueGeneratorProvider : IValueGeneratorProvider
    {
        public ValueGeneratorProvider(IServiceProvider serviceProvider, IValueGeneratorManager valueGeneratorManager)
        {
            this.serviceProvider = serviceProvider;
            valueGeneratorFactory = valueGeneratorManager;
        }

        public IValueGenerator GetValueGeneratorByName(string generatorName, bool usesDefaultServiceInjector = true)
        {
            if (usesDefaultServiceInjector && valueGeneratorFactory.TryGetValue(generatorName, out var generatorType))
            {
                return (IValueGenerator)serviceProvider.GetService(generatorType);
            }

            throw new NotSupportedException($"{generatorName} not found.");
        }
        
        
        private readonly IValueGeneratorManager valueGeneratorFactory;
        private readonly IServiceProvider serviceProvider;
    }
}
