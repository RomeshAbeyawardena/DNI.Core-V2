using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Domains;
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
        public EncryptionProfileDictionaryBuilder()
        {
            
        }

        IEncryptionProfileDictionaryBuilder IEncryptionProfileDictionaryBuilder.Add(
            EncryptionClassification encryptionClassification, 
            Func<IEncryptionProfile, IEncryptionProfile> encryptionProfileBuilder)
        {
            Add(encryptionClassification, encryptionProfileBuilder(new EncryptionProfile()));
            return this;
        }
    }
}
