using System.Collections.Generic;

namespace DNI.Core.Contracts.Managers
{
    public interface IEncryptionProfileManager : IReadOnlyDictionary<Shared.Enumerations.EncryptionClassification, IEncryptionProfile>
    {
        
    }
}
