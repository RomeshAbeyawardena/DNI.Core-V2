using DNI.Core.Contracts.Builders;
using DNI.Core.Extensions;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestWebApp;

namespace TestConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await BuildAppHost.ConfigureAppHost(appHost => appHost
                .ConfigureServices(ConfigureServices))
                .ConfigureLogging(logging => logging
                    .AddConsole()
                    .AddDatabase(serviceProvider => serviceProvider.GetRequiredService<ApplicationSettings>().DefaultConnectionString,
                    configuration =>
                        configuration
                            .ConfigureDatabaseLogManagers<DatabaseLogManager>()
                            .ConfigureLogStatusManager<LogConfiguration>(), 
                        ServiceLifetime.Singleton))
                .ConfigureAppConfiguration((host, configuration) => configuration
                    .AddJsonFile("appSettings.json")
                    .AddCommandLine(args))
                .Build<Startup>()
                .UseStartup(startingAsyncMethod: async(startup, cancellationToken) => await startup.RunAsync(cancellationToken), 
                            stoppingAsyncMethod: async(startup, cancellationToken) => await startup.StopAsync(cancellationToken))
                .StartAsync();
        }

        
        private static void ConfigureServices(HostBuilderContext arg1, IServiceCollection services)
        {
            services.AddSingleton<ApplicationSettings>();
            new ServiceRegistration().RegisterServices(services);   
        }

        static IAppHostBuilder BuildAppHost => new AppHostBuilder();
    }
}
