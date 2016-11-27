namespace MediatR.DomainEvents
{
    public interface IAggregateRepository<TAggregate>
        where TAggregate : IAggregate
    {
        void AddOrUpdate(TAggregate aggregate);

        TAggregate Get(string id);
    }

    public interface IHandleCommand
    {
        void HandleCommand(ICommand command);
    }
}