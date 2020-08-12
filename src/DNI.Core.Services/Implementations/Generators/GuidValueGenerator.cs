using DNI.Core.Contracts;
using DNI.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Implementations.Generators
{
    internal class GuidValueGenerator : IValueGenerator
    {
        public GuidValueGenerator(IGuidService guidService)
        {
            this.guidService = guidService;
        }

        public Func<object, object> GenerateValue => (value) => guidService.GenerateGuid(); 
        
        private readonly IGuidService guidService;
    }
}
