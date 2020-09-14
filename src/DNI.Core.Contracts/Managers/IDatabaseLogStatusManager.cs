using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Managers
{
    public interface IDatabaseLogStatusManager
    {
        bool IsEnabled(LogLevel logLevel);
        Task<bool> IsEnabledAsync(LogLevel logLevel);
    }

    public interface IDatabaseLogStatusManager<TLogStatus> : IDatabaseLogStatusManager
        where TLogStatus : class
    {
        
    }
}
