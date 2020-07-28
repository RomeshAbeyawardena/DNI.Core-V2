using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
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
        }

        public IValueGenerator GetValueGeneratorByName(string generatorName, bool usesDefaultServiceInjector = true)
        {
            if (usesDefaultServiceInjector)
            {
                var generatorType = Type.GetType(generatorName);

                return (IValueGenerator)serviceProvider.GetService(generatorType);
            }

            throw new NotSupportedException();
        }
        
        private readonly IServiceProvider serviceProvider;
    }
}
