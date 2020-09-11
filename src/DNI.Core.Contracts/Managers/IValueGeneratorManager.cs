using DNI.Core.Contracts.Factories;

namespace DNI.Core.Contracts.Managers
{
    public interface IValueGeneratorManager: IImplementationServiceFactory<string, IValueGenerator>
    {
        
    }
}
