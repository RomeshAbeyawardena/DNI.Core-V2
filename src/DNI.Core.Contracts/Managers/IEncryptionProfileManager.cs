using DNI.Core.Contracts.Factories;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Managers
{
    public interface IEncryptionProfileManager : IImplementationFactory<Shared.Enumerations.EncryptionClassification, IEncryptionProfile>
    {
        
    }
}
