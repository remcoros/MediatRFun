using System.Linq;
using FluentValidation;
using MediatR.FluentValidation;
using MediatR.Ninject.Configuration;
using Ninject.Extensions.Conventions;

namespace MediatR.Ninject
{
    public static class MediatorConfigurationExtensions
    {
        public static MediatorConfiguration AddValidation(this MediatorConfiguration config)
        {
            var kernel = config.Kernel;
            kernel.Bind(scan => config.FromSyntax(scan)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IValidator<>))
                .BindSelection((service, types) => new[]
                    {
                        types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))
                    }));

            kernel.Bind(typeof(IPipelineBehavior<,>))
                .To(typeof(ValidationPipelineBehavior<,>))
                .InTransientScope();

            return config;
        }
    }
}