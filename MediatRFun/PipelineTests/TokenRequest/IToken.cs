using MediatR;

namespace MediatRFun.PipelineTests.TokenRequest
{
    public interface IToken
    {
        string Token { get; set; }
    }
}