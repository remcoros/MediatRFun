using System;
using System.Linq;
using MediatR.Ninject.Resolvers;
using MediatR.Pipeline;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;
using Ninject.Planning.Bindings.Resolvers;

namespace MediatR.Ninject.Configuration
{
    public class MediatorConfiguration
    {
        public Func<IFromSyntax, ISelectSyntax> FromSyntax { get; }

        public MediatorConfiguration(IKernel kernel, Func<IFromSyntax, ISelectSyntax> from)
        {
            FromSyntax = from;
            Kernel = kernel;
        }

        public IKernel Kernel { get; }

        private MediatorConfiguration BindHandlers()
        {
            // Requests
            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IRequestHandler<>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IRequestHandler<,>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IAsyncRequestHandler<>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IAsyncRequestHandler<,>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(ICancellableAsyncRequestHandler<>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(ICancellableAsyncRequestHandler<,>))
                .BindAllInterfaces());

            // Pipeline
            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IPipelineBehavior<,>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IRequestPreProcessor<>))
                .BindSelection((service, types) => new[]
                    {
                        types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRequestPreProcessor<>))
                    }));

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IRequestPostProcessor<,>))
                .BindSelection((service, types) => new[]
                    {
                        types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRequestPostProcessor<,>))
                    }));

            // Notifications
            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IAsyncNotificationHandler<>))
                .BindAllInterfaces());

            Kernel.Bind(x => FromSyntax(x)
                .SelectAllClasses()
                .InheritedFromAny(typeof(ICancellableAsyncNotificationHandler<>))
                .BindAllInterfaces());

            return this;
        }

        public void Configure()
        {
            Kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            // _kernel.Components.Add<IBindingResolver, CovariantBindingResolver>();

            Kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>()
                .SelectAllClasses()
                .BindDefaultInterface());

            Kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>()
                .SelectAllClasses()
                .InheritedFromAny(typeof(IPipelineBehavior<,>))
                .BindAllInterfaces());

            Kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.TryGet(t));
            Kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

            BindHandlers();
        }
    }
}