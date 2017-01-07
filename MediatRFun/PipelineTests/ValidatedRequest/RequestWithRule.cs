using MediatR;

namespace MediatRFun.PipelineTests.ValidatedRequest
{
    public class RequestWithRule : IRequest
    {
        public int Value { get; set; }
    }

    public class RequestWithRuleHandler : IRequestHandler<RequestWithRule>
    {
        public void Handle(RequestWithRule message)
        {
        }
    }
}