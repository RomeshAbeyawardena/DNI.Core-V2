using DNI.Core.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Builders
{
    public interface IEncryptionProfileDictionaryBuilder : IDictionaryBuilder<EncryptionClassification, IEncryptionProfile>
    {
        IEncryptionProfileDictionaryBuilder Add (
            EncryptionClassification encryptionClassification, 
            Func<IServiceProvider ,IEncryptionProfile> encryptionProfileBuilder);
    }
}
