using MediatR.Pipeline;

namespace MediatRFun.PipelineTests.CommonHandlers
{
    public class LogHook<TRequest, TResponse> : IPostRequestHandler<TRequest, TResponse>
    {
        public void Handle(TRequest request, TResponse response)
        {
            LogHookTestValues.LogCount++;
        }
    }
}