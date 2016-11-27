namespace MediatR.DomainEvents
{
    public interface IAggregate
    {
        string Id { get; }

        int Version { get; }
    }

    public class Event : INotification
    {
        public int Version { get; set; }
    }
}