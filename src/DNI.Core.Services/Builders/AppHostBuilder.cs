using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DNI.Core.Services.Builders;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using System;
using System.Collections.Generic;
using DNI.Core.Services.Hosts;
using DNI.Core.Shared.Attributes;

namespace DNI.Core.Services.Builders
{
    [IgnoreScanning]
    public sealed class AppHostBuilder : IAppHostBuilder
    {
        public AppHostBuilder(IDictionary<object, object> properties = null)
        {
            services = new ServiceCollection();
            configurationBuilder = new ConfigurationBuilder();
            loggingBuilder = new LoggingBuilder(services);
            Properties = properties == null 
                ? new Dictionary<object, object>()
                : new Dictionary<object, object>(properties);
            context = new HostBuilderContext(Properties);
            
        }

        public AppHostBuilder(IHostBuilder hostBuilder)
            : this(hostBuilder.Properties)
        {

        }

        public IDictionary<object, object> Properties { get; }

        public IHost Build()
        {
            services
                .AddOptions()
                .AddSingleton<IConfiguration>(configurationBuilder.Build())
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddSingleton(typeof(ILoggerFactory), typeof(LoggerFactory));

            return new AppHost(services.BuildServiceProvider());
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(context, configurationBuilder);
            return this;
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            throw new NotSupportedException("ConfigureContainer not supported");
        }

        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(configurationBuilder);
            return this;
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            configureDelegate(context, services);
            return this;
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            throw new NotSupportedException("UseServiceProviderFactory not supported");
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            throw new NotSupportedException("UseServiceProviderFactory not supported");
        }

        public IAppHost<TStartup> Build<TStartup>()
            where TStartup: class
        {
            Build();
            services.AddSingleton<TStartup>();
            return new AppHost<TStartup>(services.BuildServiceProvider());
        }

        IAppHostBuilder IAppHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configurationDelegate)
        {
            ConfigureServices(configurationDelegate);
            return this;
        }

        IAppHostBuilder IAppHostBuilder.ConfigureHostConfiguration(Action<IConfigurationBuilder> configurationDelegate)
        {
            ConfigureHostConfiguration(configurationDelegate);
            return this;
        }

        IAppHostBuilder IAppHostBuilder.ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configurationDelegate)
        {
            ConfigureAppConfiguration(configurationDelegate);
            return this;
        }

        public IAppHostBuilder ConfigureLogging(Action<ILoggingBuilder> configureDelegate)
        {
            configureDelegate(loggingBuilder);
            return this;
        }

        private readonly ILoggingBuilder loggingBuilder;
        private readonly IConfigurationBuilder configurationBuilder;
        private readonly IServiceCollection services;
        private readonly HostBuilderContext context;
    }
}
