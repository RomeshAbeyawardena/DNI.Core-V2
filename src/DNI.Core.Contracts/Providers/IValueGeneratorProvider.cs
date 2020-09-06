namespace DNI.Core.Contracts.Providers
{
    public interface IValueGeneratorProvider
    {
        IValueGenerator GetValueGeneratorByName(string generatorName, bool usesDefaultServiceInjector = true);
    }
}
