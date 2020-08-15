using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Builders
{
    public class EncryptionProfileDictionaryBuilder : DictionaryBuilder<EncryptionClassification, IEncryptionProfile>, IEncryptionProfileDictionaryBuilder
    {
        public EncryptionProfileDictionaryBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEncryptionProfileDictionaryBuilder Add(
            EncryptionClassification encryptionClassification, 
            Func<IServiceProvider, IEncryptionProfile> encryptionProfileBuilder)
        {
            Add(encryptionClassification, encryptionProfileBuilder(serviceProvider));
            return this;
        }

        private readonly IServiceProvider serviceProvider;
    }
}
