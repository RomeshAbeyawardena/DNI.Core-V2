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
        public GeneratedDefaultValueAttribute(string generatorName, object defaultValue = default, bool usesDefaultServiceInjector = true)
        {
            GeneratorName = generatorName;
            DefaultValue = defaultValue;
            UsesDefaultServiceInjector = usesDefaultServiceInjector;
        }

        public string GeneratorName { get; }
        public object DefaultValue { get; }
        public bool UsesDefaultServiceInjector { get; }
    }
}
