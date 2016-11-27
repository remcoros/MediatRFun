using System;
using System.Collections.Generic;
using MediatR.DomainEvents.Internal;

namespace MediatR.DomainEvents
{
    public abstract class Aggregate<TAggregate> :
        IAggregate,
        IHandleCommand
        where TAggregate : Aggregate<TAggregate>, IAggregate
    {
        private readonly List<Event> _changes = new List<Event>();
        private readonly dynamic _self;

        protected Aggregate()
        {
            Id = Guid.NewGuid().ToString();
            _self = this.AsDynamic();
        }

        public void HandleCommand(ICommand command)
        {
            _self.Handle(command);
        }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);

            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        public string Id { get; protected set; }

        public int Version { get; protected set; }
    }
}