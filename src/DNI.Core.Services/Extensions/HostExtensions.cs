using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Services.Builders;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class HostExtensions
    {
        public static IAppHostBuilder ConfigureAppHost(this IHostBuilder hostBuilder, Action<IAppHostBuilder> configureAppHost = null)
        {
            var appHostBuilder = new AppHostBuilder(hostBuilder);
            configureAppHost?.Invoke(appHostBuilder);
            return appHostBuilder;
        }

        public static IAppHost<TStartup> UseStartup<TStartup>(
            this IAppHost<TStartup> host, 
            Action<TStartup> startingMethod = null, 
            Func<TStartup, CancellationToken, Task> startingAsyncMethod = null,
            Action<TStartup> stoppingMethod = null,
            Func<TStartup, CancellationToken, Task> stoppingAsyncMethod = null
            )
        {
            host.StartAction = startingMethod;
            host.StartActionAsync = startingAsyncMethod;
            host.StopAction = stoppingMethod;
            host.StopActionAsync = stoppingAsyncMethod;
            return host;
        }
    }
}
