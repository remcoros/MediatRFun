using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public class CancellableAsyncRequestPipeline<TRequest, TResponse> : RequestPipelineBase<TRequest, TResponse>,
        ICancellableAsyncRequestHandler<TRequest, TResponse>
        where TRequest : ICancellableAsyncRequest<TResponse>
    {

        private readonly ICancellableAsyncRequestHandler<TRequest, TResponse> _inner;

        public CancellableAsyncRequestPipeline(
            ICancellableAsyncRequestHandler<TRequest, TResponse> inner,
            IPreRequestHandler<TRequest, TResponse>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
            IResponseProcessor<TResponse>[] responseProcessor)
            : base(preRequestHandlers, postRequestHandlers, responseProcessor)
        {
            _inner = inner;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            HandleBefore(request);

            var response = await _inner.Handle(request, cancellationToken).ConfigureAwait(false);

            HandleAfter(request, response);

            return response;
        }
    }
}