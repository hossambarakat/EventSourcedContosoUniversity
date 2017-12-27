using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class StudentDeleted : Event
    {
        public StudentDeleted(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}
