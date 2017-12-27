using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class CourseUpdated : Event
    {
        public CourseUpdated(Guid id, string title, int credits, Guid departmentId)
        {
            Id = id;
            Title = title;
            Credits = credits;
            DepartmentId = departmentId;
        }
        public Guid Id { get; }
        public string Title { get; }
        public int Credits { get; }
        public Guid DepartmentId { get; }

    }
}
