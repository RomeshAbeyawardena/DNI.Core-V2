using Microsoft.Extensions.Logging;
using System;

namespace DNI.Core.Contracts.Factories
{
    public interface ILoggerCacheFactory : IImplementationFactory<Type, ILogger>
    {
        ILogger<TCategory> GetOrCreateLogger<TCategory>();
    }
}
