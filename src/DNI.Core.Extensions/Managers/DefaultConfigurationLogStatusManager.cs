using DNI.Core.Contracts;
using DNI.Core.Domains;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Extensions.Managers
{
    public class DefaultConfigurationLogStatusManager<TConfiguration> : DatabaseLogStatusManagerBase<TConfiguration>
        where TConfiguration : class, ILogStatusConfiguration
    {
        public DefaultConfigurationLogStatusManager(
            IServiceProvider serviceProvider, 
            DatabaseLoggerOptions databaseLoggerOptions) : base(serviceProvider, databaseLoggerOptions)
        {
            logStatusConfiguration =  serviceProvider.GetService(databaseLoggerOptions.ConfigurationType) as ILogStatusConfiguration;
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            if(logStatusConfiguration.LogStatus.TryGetValue(logLevel, out var status))
            {
                return status;
            }

            return true;
        }

        public override Task<bool> IsEnabledAsync(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        private ILogStatusConfiguration logStatusConfiguration;
    }
}
