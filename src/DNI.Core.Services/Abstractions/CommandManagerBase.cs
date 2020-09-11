using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using DNI.Core.Shared.Enumerations;
using DNI.Core.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class CommandManager : ImplementationFactoryBase<string, ICommand>, ICommandManager
    {
        protected CommandManager(IEnumerable<KeyValuePair<string, ICommand>> dictionaryProvider)
            : base(dictionaryProvider)
        {

        }

        IDictionary<string, ICommand> ICommandManager.Dictionary => Dictionary;

        public ICommandManager Add(string commandName, ICommand command)
        {
            if (Dictionary.TryAdd(commandName, command))
            { 
                return this;
            }

            throw new ConcurrencyException(ConcurrentAction.Add, $"Unable to add {commandName} to dictionary");
        }

        ICommandManager ICommandManager.AddCommand(string commandName, Action<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> action)
        {
            return Add(commandName, new Command(commandName, action));
        }

        ICommandManager ICommandManager.AddCommand(string commandName, Func<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> action)
        {
            return Add(commandName, new Command(commandName, action));
        }

        bool ICommandManager.TryGetCommand(string commandName, out ICommand command)
        {
            return Dictionary.TryGetValue(commandName, out command);
        }
    }
}
