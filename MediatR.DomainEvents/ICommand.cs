namespace MediatR.DomainEvents
{
    public interface ICommand<TAggregate> :
        ICommand
        where TAggregate : IAggregate
    {
    }

    public interface ICommand : IRequest<string>
    {
        string Id { get; set; }
    }
}