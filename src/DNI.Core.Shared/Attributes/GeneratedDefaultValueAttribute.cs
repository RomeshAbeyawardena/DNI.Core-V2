using System;

namespace DNI.Core.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GeneratedDefaultValueAttribute : Attribute
    {
        public GeneratedDefaultValueAttribute(
            string generatorName, 
            bool setOnUpdate = false,
            object defaultValue = default, 
            bool usesDefaultServiceInjector = true)
        {
            GeneratorName = generatorName;
            SetOnUpdate = setOnUpdate;
            DefaultValue = defaultValue;
            UsesDefaultServiceInjector = usesDefaultServiceInjector;
        }

        public string GeneratorName { get; }
        public bool SetOnUpdate { get; }
        public object DefaultValue { get; }
        public bool UsesDefaultServiceInjector { get; }
    }
}
