using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class CourseCreated : Event
    {
        public CourseCreated(Guid id,int number,string title,int credits,Guid departmentId)
        {
            Id = id;
            Number = number;
            Title = title;
            Credits = credits;
            DepartmentId = departmentId;
        }
        public Guid Id { get; }
        public int Number { get; }
        public string Title { get; }
        public int Credits { get; }
        public Guid DepartmentId { get; }
    }
}
