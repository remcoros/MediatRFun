// ReSharper disable UnusedMember.Local
using System.Collections.Generic;
using MediatR.DomainEvents;

namespace MediatRFun.DomainEvents.ChatServer
{
    public partial class ChatRoom : Aggregate<ChatRoom>
    {
        public string Title { get; private set; }

        public List<string> Users { get; } = new List<string>();

        public void Handle(Commands.CreateRoom room)
        {
            ApplyChange(new Events.RoomCreated() { RoomId = room.Id, Title = room.Title });
        }

        void Apply(Events.RoomCreated room)
        {
            Id = room.RoomId;
            Title = room.Title;
        }

        public void Handle(Commands.JoinUser command)
        {
            ApplyChange(new Events.UserJoined() { RoomId = command.Id, UserId = command.UserId});
        }

        void Apply(Events.UserJoined @event)
        {
            Users.Add(@event.UserId);
        }
    }
}