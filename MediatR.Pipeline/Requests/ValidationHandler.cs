using System.Linq;
using FluentValidation;

namespace MediatR.Pipeline
{
    public class ValidationHandler<TRequest, TResponse> :
        IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHandler;
        private readonly IValidator<TRequest>[] _validators;

        public ValidationHandler(IRequestHandler<TRequest, TResponse> innerHandler, IValidator<TRequest>[] validators)
        {
            _innerHandler = innerHandler;
            _validators = validators;
        }

        public TResponse Handle(TRequest request)
        {
            Validate(request);

            return _innerHandler.Handle(request);
        }

        protected virtual void Validate(TRequest request)
        {
            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }
    }
}