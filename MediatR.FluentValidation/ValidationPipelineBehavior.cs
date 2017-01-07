using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace MediatR.FluentValidation
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidationPipelineBehavior(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = new List<ValidationFailure>();
            foreach (var validator in _validators)
            {
                var r = validator.Validate(context);
                foreach (var error in r.Errors)
                {
                    if (error != null)
                    {
                        failures.Add(error);
                    }
                }
            }

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
