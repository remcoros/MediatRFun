using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public class AsyncRequestPipeline<TRequest, TResponse> : RequestPipelineBase<TRequest, TResponse>,
        IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {

        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;

        public AsyncRequestPipeline(
            IAsyncRequestHandler<TRequest, TResponse> inner,
            IPreRequestHandler<TRequest, TResponse>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
            IResponseProcessor<TResponse>[] responseProcessor)
            : base(preRequestHandlers, postRequestHandlers, responseProcessor)
        {
            _inner = inner;
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            HandleBefore(request);

            var response = await _inner.Handle(request).ConfigureAwait(false);

            HandleAfter(request, response);

            return response;
        }
    }
}