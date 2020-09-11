using DNI.Core.Contracts;
using DNI.Core.Contracts.ApplicationSettings;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Parser;
using DNI.Core.Domains;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Services.Parsers
{
    public class CommandParser<TApplicationSettings> : ICommandParser
        where TApplicationSettings : IConsoleApplicationSettings
    {

        public CommandParser(TApplicationSettings applicationSettings, IInputParser inputParser, ICommandManager commandManager)
        {
            this.applicationSettings = applicationSettings;
            this.inputParser = inputParser;
            this.commandManager = commandManager;
        }

        public bool TryParse<TSettings>(string input, TSettings appletSettings, out ICommand command)
        {
            command = null;

            var inputValues = inputParser.Parse(input)?.ParsedValues;

            var commandInput = inputValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(commandInput) || !commandManager.TryGetCommand(commandInput, out command))
            {
                return false;
            }

            var argumentsAndParameters = inputValues.RemoveAt(0);
            var arguments = argumentsAndParameters.Where(arg => !arg.StartsWith(applicationSettings.ParameterSeparator));
            var parameters = argumentsAndParameters.Where(arg => arg.StartsWith(applicationSettings.ParameterSeparator));

            var parameterList = new List<IParameter>();

            foreach(var parameter in parameters)
            {
                var splitNameValues = parameter.Split(applicationSettings.ParameterNameValueSeparator);

                var name = splitNameValues.FirstOrDefault();

                name = name.Replace(applicationSettings.ParameterSeparator, string.Empty);

                if(splitNameValues.Length == 2)
                {
                    parameterList.Add(new Parameter(name, splitNameValues[1]));
                }
                else
                {
                    parameterList.Add(new Parameter(name, string.Empty));
                }
            }

            command = new Command(command, parameterList.ToArray(), arguments);
;
            return true;
        }

        private readonly TApplicationSettings applicationSettings;
        private readonly IInputParser inputParser;
        private readonly ICommandManager commandManager;
    }
}
