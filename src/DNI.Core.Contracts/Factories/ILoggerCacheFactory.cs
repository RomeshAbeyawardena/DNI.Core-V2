using DNI.Core.Contracts.Factories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Factories
{
    public interface ILoggerCacheFactory : IImplementationFactory<Type, ILogger>
    {
        ILogger<TCategory> GetOrCreateLogger<TCategory>();
    }
}
