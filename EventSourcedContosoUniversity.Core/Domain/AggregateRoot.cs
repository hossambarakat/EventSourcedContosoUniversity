using System;
using System.Collections.Generic;

namespace EventSourcedContosoUniversity.Core.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<Event> _changes = new List<Event>();
        public Guid Id { get; protected set; }
        public int Version { get; internal set; }
        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }
        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }
        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
                ApplyChange(e, false);
        }
        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }
        private void ApplyChange(Event @event, bool isNew)
        {
            (this as dynamic).Apply((dynamic)@event);
            if (isNew)
                _changes.Add(@event);
        }
    }
}
