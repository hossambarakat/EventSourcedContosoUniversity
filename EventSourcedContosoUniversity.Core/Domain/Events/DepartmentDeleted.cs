using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class DepartmentDeleted : Event
    {
        public DepartmentDeleted(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}
