using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DNI.Core.Contracts.Builders
{
    public interface IAppHostBuilder : IHostBuilder
    {
        IAppHost<TStartup> Build<TStartup>()
            where TStartup: class;
        IAppHostBuilder ConfigureLogging(Action<ILoggingBuilder> loggingBuilder);
        new IAppHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configurationDelegate);
        new IAppHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configurationDelegate);
        new IAppHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configurationDelegate);
    }
}
