using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    public class Startup
    {
        private readonly IConsoleWrapper<Startup> consoleWrapper;

        public Startup(IConsoleWrapper<Startup> consoleWrapper)
        {
            this.consoleWrapper = consoleWrapper;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await consoleWrapper.WriteLineAsync("Test", true);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
