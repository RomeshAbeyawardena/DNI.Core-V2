using DNI.Core.Contracts.Factories;
using System;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Managers
{
    public interface IValueGeneratorManager: IImplementationServiceFactory<string, IValueGenerator>
    {
        
    }
}
