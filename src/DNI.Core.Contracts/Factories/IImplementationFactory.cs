﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Factories
{
    public interface IImplementationServiceFactory<TKey, TGenericImplementation> : IImplementationFactory<TKey, Type>
    {
        bool TryGetImplementation(TKey key, out TGenericImplementation implementation); 
    }

    public interface IImplementationFactory<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        
    }
}
