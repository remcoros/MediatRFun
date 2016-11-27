using MediatR.Pipeline;

namespace MediatRFun.PipelineTests
{
    public class ModifyResponseHook : IPostRequestHandler<TestRequest, TestResponse>
    {
        public void Handle(TestRequest request, TestResponse response)
        {
            response.Message = "ModifyResponseHook";
        }
    }
}