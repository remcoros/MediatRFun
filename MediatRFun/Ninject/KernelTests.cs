using System.Linq;
using FluentAssertions;
using MediatR.Ninject;
using MediatR.Ninject.Resolvers;
using MediatR.Pipeline;
using MediatRFun.PipelineTests;
using MediatRFun.PipelineTests.TokenRequest;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Planning.Bindings.Resolvers;
using Xunit;

namespace MediatRFun.Ninject
{
    public class KernelTests
    {
        [Fact]
        public void ResolveOpenGeneric()
        {
            var kernel = new StandardKernel();
            kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            // kernel.Components.Add<IBindingResolver, CovariantBindingResolver>();

            kernel.Bind(x => x.FromAssemblyContaining<KernelTests>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IPreRequestHandler<,>)).BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPreRequestHandler<,>)) }));

            kernel.Bind(x => x.FromAssemblyContaining<KernelTests>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IPostRequestHandler<,>)).BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPostRequestHandler<,>)) }));

            kernel.Bind(x => x.FromAssemblyContaining<KernelTests>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IResponseProcessor<>)).BindSelection((service, types) => new[] { types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IResponseProcessor<>)) }));

            var h = kernel.GetAll<IPreRequestHandler<TestTokenRequest, TestResponse>>();

            h.Count().Should().Be(2);

            var h2 = kernel.GetAll<IPostRequestHandler<TestTokenRequest, object>>();
            h2.Count().Should().Be(2);

            var h3 = kernel.GetAll<IResponseProcessor<TestResponse>>();
            h3.Count().Should().Be(0);
        }
    }
}