using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DNI.Core.Contracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TestWebApp
{
    
        public class LogConfiguration : ILogStatusConfiguration
        {
            public LogConfiguration(IConfiguration configuration)
            {
                configuration.Bind(this);
            }

            //public IDictionary<string, bool> LogLevel { get; set; }

            public IDictionary<LogLevel, bool> LogStatus { get; set; }

        }
    
}
