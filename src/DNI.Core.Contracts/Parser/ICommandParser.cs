namespace DNI.Core.Contracts.Parser
{
    public interface ICommandParser
    {
        bool TryParse<TSettings>(string input, TSettings settings, out ICommand command);
    }
}
