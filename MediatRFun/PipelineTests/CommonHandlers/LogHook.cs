using System.Threading.Tasks;
using MediatR.Pipeline;

namespace MediatRFun.PipelineTests.CommonHandlers
{
    public class LogHook<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        public Task Process(TRequest request, TResponse response)
        {
            LogHookTestValues.LogCount++;
            return Task.CompletedTask;
        }
    }
}