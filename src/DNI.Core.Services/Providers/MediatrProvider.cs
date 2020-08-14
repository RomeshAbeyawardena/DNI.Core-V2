using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
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
