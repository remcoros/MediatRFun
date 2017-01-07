using System.Linq;
using FluentAssertions;
using FluentValidation;
using MediatR;
using MediatR.DomainEvents;
using MediatR.Ninject;
using MediatR.Pipeline;
using MediatRFun.DomainEvents.ChatServer;
using MediatRFun.PipelineTests;
using Ninject;
using Ninject.Extensions.Conventions;
using Xunit;
using Xunit.Abstractions;

namespace MediatRFun.DomainEvents
{
    public class DomainEventTests
    {
        private readonly ITestOutputHelper _output;
        private StandardKernel _kernel;

        public DomainEventTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ChatRoomSample()
        {
            var mediatr = CreateMediator();
            _kernel.Bind(typeof(IAggregateRepository<>)).To(typeof(InMemoryRepository<>)).InSingletonScope();

            mediatr.Send(new ChatRoom.Commands.CreateRoom() { Id = "test", Title = "Test" });
            mediatr.Send(new ChatUser.Commands.CreateUser() { Id = "user1", Name = "User" });
            mediatr.Send(new ChatRoom.Commands.JoinUser() { Id = "test", UserId = "user1" });

            var roomStore = _kernel.Get<IAggregateRepository<ChatRoom>>();
            var userStore = _kernel.Get<IAggregateRepository<ChatUser>>();
            var room = roomStore.Get("test");
            var user = userStore.Get("user1");

            user.Should().NotBeNull();
            user.Id.Should().Be("user1");
            user.Name.Should().Be("User");

            room.Should().NotBeNull();
            room.Id.Should().Be("test");
            room.Title.Should().Be("Test");
            room.Users.Should().Contain("user1");
        }

        private IMediator CreateMediator()
        {
            _kernel = new StandardKernel();
            _kernel.AddMediatR(x => x.FromAssemblyContaining<SimplePipelineTests>())
                .AddValidation()
                .AddDomainEvents();

            var mediatr = _kernel.Get<IMediator>();
            return mediatr;
        }
    }
}