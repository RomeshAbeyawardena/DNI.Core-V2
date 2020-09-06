using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ResponseRequestHandler<T, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, IResponse<T>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected ResponseRequestHandler(IMapperProvider mapperProvider)
        {
            MapperProvider = mapperProvider;
        }

        protected IMapperProvider MapperProvider { get; }

        protected virtual Task<TResponse> SuccessfulResponse(T result)
        {
            return Task.FromResult(CreateResponse(result));
        }

        protected virtual Task<TResponse> FailedResponse(Exception exception)
        {
            return Task.FromResult(CreateResponse(exception));
        }

        private TResponse CreateResponse(T result)
        {
            return Activator.CreateInstance(typeof(TResponse), result) as TResponse;
        }

        private TResponse CreateResponse(Exception exception)
        {
            return Activator.CreateInstance(typeof(TResponse), exception) as TResponse;
        }
    }
}
