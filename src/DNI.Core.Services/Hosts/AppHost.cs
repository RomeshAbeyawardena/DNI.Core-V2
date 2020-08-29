using Microsoft.Extensions.DependencyInjection;
using DNI.Core.Contracts;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DNI.Core.Shared.Attributes;

namespace DNI.Core.Services.Hosts
{
    [IgnoreScanning]
    public sealed class AppHost<TStartup>: AppHost, IAppHost<TStartup>
    {
        public Action<TStartup> StartAction { get; set; }
        public Func<TStartup, CancellationToken, Task> StartActionAsync { get; set; }
        public Action<TStartup> StopAction { get; set; }
        public Func<TStartup, CancellationToken, Task> StopActionAsync { get; set; }

        public AppHost(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            startupService = Services.GetRequiredService<TStartup>();

            if(startupService == null)
            {
                throw new NullReferenceException(nameof(startupService));
            }

            if(StartAction != null)
            {
                StartAction.Invoke(startupService);
                return Task.CompletedTask;
            }

            return StartActionAsync?.Invoke(startupService, cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken = default)
        {
            if(startupService == null)
            {
                throw new NullReferenceException(nameof(startupService));
            }

            if(StopAction != null)
            {
                StopAction.Invoke(startupService);
                return Task.CompletedTask;
            }

            return StopActionAsync?.Invoke(startupService, cancellationToken);
        }

        private TStartup startupService;
    }

    [IgnoreScanning]
    public class AppHost : IAppHost
    {
        public Type StartupType { get; set; }
        public string StartMethod { get; set; }
        public string StartAsyncMethod { get; set; }
        public IServiceProvider Services { get; }

        public AppHost(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual Task StartAsync(CancellationToken cancellationToken = default)
        {
            var startupService = Services.GetRequiredService(StartupType);

            if(startupService == null)
            {
                throw new NullReferenceException(nameof(startupService));
            }

            var method = GetMethod(StartupType, StartMethod);
            var async = false; 
            if(method == null)
            {
               method = GetMethod(StartupType, StartAsyncMethod);
               async = true;
            }

            var result = method.Invoke(startupService, null);

            if (async)
            {
                return result as Task;
            }

            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool gc)
        {
            GC.SuppressFinalize(this);
        }

        private MethodInfo GetMethod(Type type, string name)
        {
            return type.GetMethod(name);
        }
    }
}
