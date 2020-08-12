using DNI.Core.Contracts;
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
            valueGeneratorTypeDictionary = serviceProvider.GetRequiredService<Dictionary<string, Type>>();
        }

        public IValueGenerator GetValueGeneratorByName(string generatorName, bool usesDefaultServiceInjector = true)
        {
            if (usesDefaultServiceInjector && valueGeneratorTypeDictionary.TryGetValue(generatorName, out var generatorType))
            {
                return (IValueGenerator)serviceProvider.GetService(generatorType);
            }

            throw new NotSupportedException();
        }
        
        private readonly Dictionary<string, Type> valueGeneratorTypeDictionary;
        private readonly IServiceProvider serviceProvider;
    }
}
