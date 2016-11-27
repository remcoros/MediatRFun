using MediatR;

namespace MediatRFun.PipelineTests.TokenRequest
{
    public class TestTokenRequest : IRequest<TestResponse>, IToken
    {
        public string Token { get; set; }
    }

    public class TestTokenWithStringResponseRequest : IRequest<string>, IToken
    {
        public string Token { get; set; }
    }
}