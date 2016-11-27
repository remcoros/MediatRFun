using System;

namespace MediatR.Pipeline
{
    public abstract class DelegatingRequestHandler<TRequest, TResponse> :
        IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Func<TRequest, TResponse> _next;

        protected DelegatingRequestHandler(IRequestHandler<TRequest, TResponse> innerHandler)
        {
            _next = innerHandler.Handle;
        }

        public TResponse Handle(TRequest message) => Handle(message, _next);

        protected abstract TResponse Handle(TRequest request, Func<TRequest, TResponse> next);
    }
}