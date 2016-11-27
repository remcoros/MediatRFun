using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.DomainEvents
{
    //public class CommandHandler<TAggregate, TId> :
    //    RequestHandler<ICommand<TAggregate, TId>>
    //    where TAggregate : IHandleCommand<ICommand<TAggregate, TId>, TAggregate, TId>, IAggregate<TId>
    //{
    //    private readonly IAggregateRepository<TAggregate, TId> _repository;

    //    public CommandHandler(IAggregateRepository<TAggregate, TId> repository)
    //    {
    //        _repository = repository;
    //    }

    //    protected override void HandleCore(ICommand<TAggregate, TId> message)
    //    {
    //        var root = _repository.Get(message.Id);
    //        root.HandleCommand(message);
    //    }
    //}

    //public class CommandHandler2<TRequest, TResponse>
    //    : IRequestHandler<TRequest, TResponse>
    //    where TRequest : IRequest<TResponse>
    //{
    //    public TResponse Handle(TRequest message)
    //    {
    //        return default(TResponse);
    //    }
    //}

    public class CommandHandler<TRequest, TResponse, TAggregate>
            : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICommand<TAggregate>
        where TAggregate : Aggregate<TAggregate>, new()
    {
        private readonly IAggregateRepository<TAggregate> _repository;

        public CommandHandler(IAggregateRepository<TAggregate> repository)
        {
            _repository = repository;
        }

        //protected override void HandleCore(TRequest message)
        //{
        //    var root = _repository.Get(message.Id);
        //    ((IHandleCommand<ICommand<IAggregate>, IAggregate>)root).HandleCommand(message);
        //}

        public TResponse Handle(TRequest message)
        {
            var root = _repository.Get(message.Id);
            if (root == null)
            {
                root = new TAggregate();
            }

            root.HandleCommand(message);

            _repository.AddOrUpdate(root);
            root.MarkChangesAsCommitted();

            return default(TResponse);
        }
    }

    //public class CommandHandler<TRequest, TAggregate>
    //    : RequestHandler<TRequest>
    //    where TRequest : ICommand<TAggregate, string>, IRequest
    //    where TAggregate : IAggregate<string>
    //{
    //    private readonly IAggregateRepository<TAggregate> _repository;

    //    public CommandHandler(IAggregateRepository<TAggregate> repository)
    //    {
    //        _repository = repository;
    //    }

    //    protected override void HandleCore(TRequest request)
    //    {
    //        var root = (IHandleCommand<ICommand<TAggregate, string>, TAggregate, string>)_repository.Get(request.Id);
    //        root.HandleCommand(request);
    //    }
    //}

    //public class CommandHandler<TAggregate, TId, TResponse> :
    //    IRequestHandler<IDomainCommand, TResponse>
    //    where TAggregate : IHandleCommand<ICommand<TAggregate, TId, TResponse>, TAggregate, TId, TResponse>, IAggregate<TId>
    //{
    //    private readonly IAggregateRepository<TAggregate, TId> _repository;

    //    public CommandHandler(IAggregateRepository<TAggregate, TId> repository)
    //    {
    //        _repository = repository;
    //    }

    //    public TResponse Handle(ICommand<TAggregate, TId, TResponse> message)
    //    {
    //        var root = _repository.Get(message.Id);
    //        return root.HandleCommand(message);
    //    }
    //}
}
