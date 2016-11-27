namespace MediatR.Pipeline
{
    public class RequestPipeline<TRequest, TResponse> : RequestPipelineBase<TRequest, TResponse>,
            IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {

        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public RequestPipeline(
                IRequestHandler<TRequest, TResponse> inner,
                IPreRequestHandler<TRequest, TResponse>[] preRequestHandlers,
                IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
                IResponseProcessor<TResponse>[] responseProcessor)
            : base(preRequestHandlers, postRequestHandlers, responseProcessor)
        {
            _inner = inner;
        }

        public TResponse Handle(TRequest request)
        {
            HandleBefore(request);

            var response = _inner.Handle(request);

            HandleAfter(request, response);

            return response;
        }
    }
}