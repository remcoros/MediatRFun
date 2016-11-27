using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public abstract class RequestPipelineBase<TRequest, TResponse>
    {
        private readonly IPreRequestHandler<TRequest, TResponse>[] _preRequestHandlers;
        private readonly IPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;
        private readonly IResponseProcessor<TResponse>[] _responseProcessors;

        public RequestPipelineBase(
            IPreRequestHandler<TRequest, TResponse>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
            IResponseProcessor<TResponse>[] responseProcessor)
        {
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
            _responseProcessors = responseProcessor;
        }

        protected void HandleBefore(TRequest request)
        {
            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(request);
            }
        }

        protected void HandleAfter(TRequest request, TResponse response)
        {
            foreach (var postRequestHandler in _postRequestHandlers)
            {
                postRequestHandler.Handle(request, response);
            }

            foreach (var responseProcessor in _responseProcessors)
            {
                responseProcessor.Handle(response);
            }
        }
    }
}