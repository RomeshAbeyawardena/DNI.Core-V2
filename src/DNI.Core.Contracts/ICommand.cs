using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ICommand
    {
        public string Name { get; }
        Action<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> Action { get; }
        Func<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> ActionAsync { get; }
        IEnumerable<IParameter> Parameters { get; }
        IEnumerable<string> Arguments { get; }
    }
}
