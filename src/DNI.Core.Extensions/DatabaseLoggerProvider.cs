using System;
using DNI.Core.Domains;
using Microsoft.Extensions.Logging;

namespace DNI.Core.Extensions
{
    public sealed class DatabaseLoggerProvider : ILoggerProvider
    {
        public DatabaseLoggerProvider(IServiceProvider serviceProvider, DatabaseLoggerOptions databaseLoggerOptions)
        {
            this.serviceProvider = serviceProvider;
            this.databaseLoggerOptions = databaseLoggerOptions;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var genericLoggerType = databaseLoggerOptions.GetGenericType();
            return serviceProvider.GetService(genericLoggerType) as ILogger;
        }

        public void Dispose()
        {
        }

        private IServiceProvider serviceProvider;
        private readonly DatabaseLoggerOptions databaseLoggerOptions;
    }
}
