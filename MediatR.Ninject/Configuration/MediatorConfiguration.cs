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
        private static readonly Type _openGenericRequestHandlerType = typeof(IRequestHandler<,>);
        private static readonly Type _openGenericAsyncRequestHandlerType = typeof(IAsyncRequest<>);
        private static readonly Type _openGenericCancellableAsyncRequestHandlerType = typeof(ICancellableAsyncRequest<>);
        private static readonly Type _openGenericNotificationHandlerType = typeof(INotificationHandler<>);
        private static readonly Type _openGenericAsyncNotificationHandlerType = typeof(IAsyncNotificationHandler<>);
        private static readonly Type _openGenericCancellableAsyncNotificationHandlerType = typeof(ICancellableAsyncNotificationHandler<>);
        private static readonly Type _mediatorPipelineType = typeof(RequestPipeline<,>);
        private static readonly Type _mediatorAsyncPipelineType = typeof(AsyncRequestPipeline<,>);
        private static readonly Type _mediatorCancellableAsyncPipelineType = typeof(CancellableAsyncRequestPipeline<,>);
        private static readonly Type _mediatorNotificationPipelineType = typeof(RequestPipeline<,>);
        private static readonly Type _mediatorAsyncNotificationPipelineType = typeof(AsyncRequestPipeline<,>);
        private static readonly Type _mediatorCancellableAsyncNotificationPipelineType = typeof(CancellableAsyncRequestPipeline<,>);

        private Type _decoratedPipelineType = typeof(RequestPipeline<,>);

        public MediatorConfiguration(IKernel kernel)
        {
            Kernel = kernel;
        }

        public IKernel Kernel { get; }

        public MediatorConfiguration DecoratePipeline(Type type)
        {
            Kernel.Rebind(typeof(IRequestHandler<,>)).To(type);
            Kernel.Bind(typeof(IRequestHandler<,>)).To(_decoratedPipelineType).WhenInjectedInto(type);
            _decoratedPipelineType = type;

            return this;
        }

        public MediatorConfiguration BindHandlers(Func<IFromSyntax, ISelectSyntax> from)
        {
            // Requests
            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFromAny(_openGenericRequestHandlerType)
                .BindAllInterfaces()
                .Configure(syntax => syntax.WhenInjectedInto(_mediatorPipelineType)));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFromAny(_openGenericAsyncRequestHandlerType)
                .BindAllInterfaces()
                .Configure(syntax => syntax.WhenInjectedInto(_mediatorAsyncPipelineType)));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFromAny(_openGenericCancellableAsyncRequestHandlerType)
                .BindAllInterfaces()
                .Configure(syntax => syntax.WhenInjectedInto(_mediatorCancellableAsyncPipelineType)));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IPreRequestHandler<>))
                .BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPreRequestHandler<>)) }));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IPreRequestHandler<,>))
                .BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPreRequestHandler<,>)) }));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IPostRequestHandler<,>))
                .BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPostRequestHandler<,>)) }));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IResponseProcessor<>))
                .BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IResponseProcessor<>)) }));

            // Notifications
            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFromAny(_openGenericNotificationHandlerType)
                .BindAllInterfaces()
                .Configure(syntax => syntax.WhenInjectedInto(_mediatorNotificationPipelineType)));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFromAny(_openGenericAsyncNotificationHandlerType)
                .BindAllInterfaces()
                .Configure(syntax => syntax.WhenInjectedInto(_mediatorAsyncNotificationPipelineType)));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFromAny(_openGenericCancellableAsyncNotificationHandlerType)
                .BindAllInterfaces()
                .Configure(syntax => syntax.WhenInjectedInto(_mediatorCancellableAsyncNotificationPipelineType)));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IPreNotificationHandler<>))
                .BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPreNotificationHandler<>)) }));

            Kernel.Bind(x => from(x)
                .SelectAllClasses()
                .InheritedFrom(typeof(IPostNotificationHandler<>))
                .BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPostNotificationHandler<>)) }));

            return this;
        }

        public void Configure()
        {
            Kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            // _kernel.Components.Add<IBindingResolver, CovariantBindingResolver>();

            Kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>().SelectAllClasses().BindDefaultInterface());
            Kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            Kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

            Kernel.Bind(typeof(IRequestHandler<,>)).To(typeof(RequestPipeline<,>));
            Kernel.Bind(typeof(IAsyncRequestHandler<,>)).To(typeof(AsyncRequestPipeline<,>));
            Kernel.Bind(typeof(ICancellableAsyncRequestHandler<,>)).To(typeof(CancellableAsyncRequestPipeline<,>));

            // BindHandlers(x => x.FromAssemblyContaining(typeof(RequestPipeline<,>)));
        }
    }
}