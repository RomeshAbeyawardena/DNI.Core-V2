using DNI.Core.Contracts;
using DNI.Core.Contracts.Parser;
using DNI.Core.Domains;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Services.Parsers
{
    public class InputParser : IInputParser
    {
        public InputParser(IInputParserOptions inputParserOptions)
        {
            if(inputParserOptions == null)
            {
                inputParserOptions = InputParserOptions.Default;
            }

            this.Options = inputParserOptions;
        }

        public IInputGroup Parse(string input, IInputParserOptions inputParserOptions = null)
        {
            if(inputParserOptions == null)
            {
                inputParserOptions = Options;
            }

            bool withinQuotes = false;
            var currentWord = string.Empty;
            var parsedInputList = new List<string>();
            for (var index = 1; index <= input.Length; index++)
            {
                var inputCharacter = input[index - 1];

                if(inputParserOptions.InputQuoteGroups.Any(groupCharacter => groupCharacter == inputCharacter))
                {
                    withinQuotes = !withinQuotes;
                    continue;
                }

                if(!withinQuotes && inputParserOptions.InputSeparatorGroups.Any(groupCharacter => groupCharacter == inputCharacter))
                {
                    parsedInputList.Add(currentWord);
                    currentWord =  string.Empty;
                    continue;
                }

                currentWord += inputCharacter;
            }

            parsedInputList.Add(currentWord);

            return new InputGroup(parsedInputList.ToArray());
        }

        public IInputParserOptions Options { get; }
    }
}
