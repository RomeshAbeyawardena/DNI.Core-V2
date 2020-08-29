using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IAppHost : IHost
    {
        Type StartupType { get; set; }
        string StartMethod { get; set; }
        string StartAsyncMethod { get; set; }
    }

    public interface IAppHost<TStartup> : IAppHost
    {
        Action<TStartup> StartAction { get; set; }
        Func<TStartup, CancellationToken, Task> StartActionAsync { get; set; }
        Action<TStartup> StopAction { get; set; }
        Func<TStartup, CancellationToken, Task> StopActionAsync { get; set; }
    }
}
