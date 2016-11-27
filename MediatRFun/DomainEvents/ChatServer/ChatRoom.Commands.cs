using MediatR.DomainEvents;

namespace MediatRFun.DomainEvents.ChatServer
{
    public partial class ChatRoom
    {
        public static partial class Commands
        {
            public class CreateRoom : ICommand<ChatRoom>
            {
                public string Id { get; set; }

                public string Title { get; set; }
            }

            public class JoinUser : ICommand<ChatRoom>
            {
                public string Id { get; set; }

                public string UserId { get; set; }
            }
        }
    }
}