using MediatR.DomainEvents;

namespace MediatRFun.DomainEvents.ChatServer
{
    public partial class ChatRoom
    {
        public static partial class Events
        {
            public class RoomCreated : Event
            {
                public string RoomId { get; set; }

                public string Title { get; set; }
            }

            public class UserJoined : Event
            {
                public string RoomId { get; set; }

                public string UserId { get; set; }
            }
        }
    }
}