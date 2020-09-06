using DNI.Core.Shared.Enumerations;
using System;

namespace DNI.Core.Contracts.Builders
{
    public interface IEncryptionProfileDictionaryBuilder : IDictionaryBuilder<EncryptionClassification, IEncryptionProfile>
    {
        IEncryptionProfileDictionaryBuilder Add (
            EncryptionClassification encryptionClassification, 
            Func<IEncryptionProfile, IEncryptionProfile> encryptionProfileBuilder);
    }
}
