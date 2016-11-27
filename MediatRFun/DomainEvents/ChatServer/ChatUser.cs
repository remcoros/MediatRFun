using MediatR.DomainEvents;

namespace MediatRFun.DomainEvents.ChatServer
{
    public partial class ChatUser : Aggregate<ChatUser>
    {
        public string Name { get; set; }

        public void Handle(Commands.CreateUser command)
        {
            ApplyChange(new Events.UserCreated() { UserId = command.Id, Name = command.Name });
        }

        void Apply(Events.UserCreated @event)
        {
            Id = @event.UserId;
            Name = @event.Name;
        }

        public static partial class Commands
        {
            public class CreateUser : ICommand<ChatUser>
            {
                public string Id { get; set; }

                public string Name { get; set; }
            }
        }

        public static partial class Events
        {
            public class UserCreated : Event
            {
                public string UserId { get; set; }

                public string Name { get; set; }
            }
        }
    }
}