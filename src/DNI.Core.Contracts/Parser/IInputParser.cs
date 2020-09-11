namespace DNI.Core.Contracts.Parser
{
    public interface IInputParser
    {
        IInputGroup Parse(string input, IInputParserOptions inputParserOptions = null);
        IInputParserOptions Options { get; }
    }
}
