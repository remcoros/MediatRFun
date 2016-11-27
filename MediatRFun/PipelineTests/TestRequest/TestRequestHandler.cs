using MediatR;

namespace MediatRFun.PipelineTests
{
    public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public TestResponse Handle(TestRequest message)
        {
            return new TestResponse()
                {
                    Message = "TestResponse"
                };
        }
    }
}