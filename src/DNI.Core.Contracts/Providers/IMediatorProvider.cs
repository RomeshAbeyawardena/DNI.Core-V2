using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IMediatorProvider
    {
        Task Publish(object notification, CancellationToken cancellationToken = default);
        Task<object> Send(object request, CancellationToken cancellationToken = default);
    }
}
