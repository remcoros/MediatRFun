using FluentValidation;

namespace MediatRFun.PipelineTests.ValidatedRequest
{
    public class RequestWithRuleValidator : AbstractValidator<RequestWithRule>
    {
        public RequestWithRuleValidator()
        {
            RuleFor(m => m.Value).GreaterThan(0);
        }
    }
}