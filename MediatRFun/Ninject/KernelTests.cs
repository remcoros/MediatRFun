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
                .InheritedFrom(typeof(IRequestPreProcessor<>))
                .BindSelection((service, types) => new[]
                    {
                        types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRequestPreProcessor<>))
                    }));

            kernel.Bind(x => x.FromAssemblyContaining<KernelTests>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IRequestPostProcessor<,>))
                .BindSelection((service, types) => new[]
                    {
                        types.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRequestPostProcessor<,>))
                    }));

            var h = kernel.GetAll<IRequestPreProcessor<TestTokenRequest>>();

            h.Count().Should().Be(1);

            var h2 = kernel.GetAll<IRequestPostProcessor<TestTokenRequest, object>>();
            h2.Count().Should().Be(2);
        }
    }
}