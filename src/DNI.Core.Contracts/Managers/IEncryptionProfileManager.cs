using DNI.Core.Contracts.Factories;

namespace DNI.Core.Contracts.Managers
{
    public interface IEncryptionProfileManager : IImplementationFactory<Shared.Enumerations.EncryptionClassification, IEncryptionProfile>
    {
        
    }
}
