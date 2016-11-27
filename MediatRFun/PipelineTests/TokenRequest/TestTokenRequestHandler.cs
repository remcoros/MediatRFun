using MediatR;

namespace MediatRFun.PipelineTests.TokenRequest
{
    public class TestTokenRequestHandler :
        IRequestHandler<TestTokenRequest, TestResponse>,
        IRequestHandler<TestTokenWithStringResponseRequest, string>
    {
        public TestResponse Handle(TestTokenRequest message)
        {
            return new TestResponse();
        }

        public string Handle(TestTokenWithStringResponseRequest message)
        {
            return "TestTokenRequestHandler";
        }
    }
}