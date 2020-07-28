using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GeneratedDefaultValueAttribute : Attribute
    {
        public GeneratedDefaultValueAttribute(string generatorName, bool usesDefaultServiceInjector = true)
        {
            GeneratorName = generatorName;
            UsesDefaultServiceInjector = usesDefaultServiceInjector;
        }

        public string GeneratorName { get; }
        public bool UsesDefaultServiceInjector { get; }
    }
}
