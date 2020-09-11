using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Services.Abstractions;
using System;
using System.Collections.Generic;

namespace DNI.Core.Services.Managers
{
    internal class ValueGeneratorManager : ImplementationServiceFactoryBase<string, IValueGenerator>, IValueGeneratorManager
    {
        public ValueGeneratorManager(IServiceProvider serviceProvider, IEnumerable<KeyValuePair<string, Type>> valueKeyPairs)
            : base(serviceProvider, valueKeyPairs)
        {
            
        }

    }
}
