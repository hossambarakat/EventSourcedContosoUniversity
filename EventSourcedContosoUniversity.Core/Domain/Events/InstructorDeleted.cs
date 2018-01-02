using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class InstructorDeleted : Event
    {
        public InstructorDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
