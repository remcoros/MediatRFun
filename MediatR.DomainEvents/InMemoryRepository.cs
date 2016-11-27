using System;
using System.Collections.Generic;

namespace MediatR.DomainEvents
{
    public class InMemoryRepository<TAggregate> : IAggregateRepository<TAggregate>
        where TAggregate : Aggregate<TAggregate>
    {
        private readonly IDictionary<Type, IDictionary<string, TAggregate>> _aggregates = new Dictionary<Type, IDictionary<string, TAggregate>>();

        public void AddOrUpdate(TAggregate aggregate)
        {
            IDictionary<string, TAggregate> aggregatesById;
            if (!_aggregates.TryGetValue(typeof(TAggregate), out aggregatesById))
            {
                _aggregates[typeof(TAggregate)] = new Dictionary<string, TAggregate>();
            }

            _aggregates[typeof(TAggregate)][aggregate.Id] = aggregate;
        }

        public TAggregate Get(string id)
        {
            IDictionary<string, TAggregate> aggregatesById;
            if (_aggregates.TryGetValue(typeof(TAggregate), out aggregatesById))
            {
                TAggregate aggregate;
                if (aggregatesById.TryGetValue(id, out aggregate))
                {
                    return aggregate;
                }
            }

            return default(TAggregate);
        }
    }
}