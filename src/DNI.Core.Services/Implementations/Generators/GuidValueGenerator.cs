using DNI.Core.Contracts;
using DNI.Core.Contracts.Services;
using DNI.Core.Shared.Attributes;
using System;

namespace DNI.Core.Services.Implementations.Generators
{
    [IgnoreScanning]
    internal class GuidValueGenerator : IValueGenerator
    {
        public GuidValueGenerator(IGuidService guidService)
        {
            this.guidService = guidService;
        }

        public Func<object, object> GenerateValue => (value) => guidService.GenerateGuid(); 
        
        public bool ExpectsValue => false;

        private readonly IGuidService guidService;
    }
}
