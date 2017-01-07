using System.Threading.Tasks;
using MediatR.Pipeline;

namespace MediatRFun.PipelineTests
{
    public class ModifyResponseHook : IRequestPostProcessor<TestRequest, TestResponse>
    {
        public Task Process(TestRequest request, TestResponse response)
        {
            response.Message = "ModifyResponseHook";
            return Task.CompletedTask;
        }
    }
}