using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class CourseDeleted : Event
    {
        public CourseDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
