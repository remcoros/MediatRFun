using MediatR;

namespace MediatRFun.PipelineTests.ValidatedRequest
{
    public class RequestWithRule : IRequest
    {
        public int Value { get; set; }
    }

    public class RequestWithRuleHandler : RequestHandler<RequestWithRule>
    {
        protected override void HandleCore(RequestWithRule message)
        {

        }
    }
}