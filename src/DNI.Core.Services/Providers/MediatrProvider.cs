using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Shared.Attributes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    [IgnoreScanning]
    internal class MediatrProvider : IMediatorProvider
    {
        public MediatrProvider(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return mediator.Publish(notification, cancellationToken);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }
        
        public Task<IResponse<T>> Send<T>(IRequest<IResponse<T>> request, CancellationToken cancellationToken)
        {
            return mediator.Send(request);
        }

        private readonly IMediator mediator;
    }
}
