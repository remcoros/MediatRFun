using FluentAssertions;
using FluentValidation;
using MediatR;
using MediatR.Ninject;
using MediatR.Pipeline;
using MediatRFun.PipelineTests.CommonHandlers;
using MediatRFun.PipelineTests.TokenRequest;
using MediatRFun.PipelineTests.ValidatedRequest;
using Ninject;
using Ninject.Extensions.Conventions;
using Xunit;
using Xunit.Abstractions;

namespace MediatRFun.PipelineTests
{
    public class SimplePipelineTests
    {
        private readonly ITestOutputHelper _output;
        private StandardKernel _kernel;

        public SimplePipelineTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestRequest()
        {
            ResetTestValues();

            var mediatr = CreateMediator();
            var h = _kernel.Get<IRequestHandler<TestRequest, TestResponse>>();

            var response = mediatr.Send(new TestRequest());

            response.Should().NotBeNull();
            response.Message.Should().Be("ModifyResponseHook");

            LogHookTestValues.LogCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestTokenRequest()
        {
            ResetTestValues();

            var mediatr = CreateMediator();
            var response = mediatr.Send(new TestTokenRequest());

            HasTokenRequestHook.PreHandleCount.Should().Be(1);
            HasTokenRequestHook.PostHandleCount.Should().Be(1);
            HasTokenRequestHook.ProcessResponseCount.Should().Be(1);

            HasTokenWithTestResponseRequestHook.PreHandleCount.Should().Be(1);
            HasTokenWithTestResponseRequestHook.PostHandleCount.Should().Be(1);
            HasTokenWithTestResponseRequestHook.ProcessResponseCount.Should().Be(1);

            HasTokenWithResponseStringRequestHook.PreHandleCount.Should().Be(0);
            HasTokenWithResponseStringRequestHook.PostHandleCount.Should().Be(0);
            HasTokenWithResponseStringRequestHook.ProcessResponseCount.Should().Be(0);
        }

        [Fact]
        public void TestTokenWithStringResponseRequest()
        {
            ResetTestValues();

            var mediatr = CreateMediator();
            var response = mediatr.Send(new TestTokenWithStringResponseRequest());

            HasTokenRequestHook.PreHandleCount.Should().Be(1);
            HasTokenRequestHook.PostHandleCount.Should().Be(1);
            HasTokenRequestHook.ProcessResponseCount.Should().Be(1);

            HasTokenWithTestResponseRequestHook.PreHandleCount.Should().Be(0);
            HasTokenWithTestResponseRequestHook.PostHandleCount.Should().Be(0);
            HasTokenWithTestResponseRequestHook.ProcessResponseCount.Should().Be(0);

            HasTokenWithResponseStringRequestHook.PreHandleCount.Should().Be(1);
            HasTokenWithResponseStringRequestHook.PostHandleCount.Should().Be(1);
            HasTokenWithResponseStringRequestHook.ProcessResponseCount.Should().Be(1);
        }

        [Fact]
        public void ValidatedRequest()
        {
            ResetTestValues();

            var mediatr = CreateMediator();
            Assert.Throws<ValidationException>(() =>
                {
                    mediatr.Send(new RequestWithRule());
                });

            var response = mediatr.Send(new RequestWithRule()
            {
                Value = 1
            });
        }

        private void ResetTestValues()
        {
            // TODO: find a nicer way to handle this without mocking/fiddling with the default container registration (we WANT to test the default registration)
            HasTokenRequestHook.Reset();
            HasTokenWithTestResponseRequestHook.Reset();
            HasTokenWithResponseStringRequestHook.Reset();
        }

        private IMediator CreateMediator()
        {
            _kernel = new StandardKernel();
            _kernel.AddMediatR(c =>
                c.BindHandlers(x => x.FromAssemblyContaining<SimplePipelineTests>())
                .AddDomainEvents()
                .DecoratePipeline(typeof(ValidationHandler<,>)));

            _kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom(typeof(IValidator<>)).BindAllInterfaces());

            var mediatr = _kernel.Get<IMediator>();
            return mediatr;
        }
    }
}
