using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IValueGeneratorProvider
    {
        IValueGenerator GetValueGeneratorByName(string generatorName, bool usesDefaultServiceInjector = true);
    }
}
