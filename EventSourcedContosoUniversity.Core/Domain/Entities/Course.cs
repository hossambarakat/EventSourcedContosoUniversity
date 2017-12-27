using System;
using EventSourcedContosoUniversity.Core.Domain.Events;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Course : AggregateRoot
    {
        private Course() { }
        public Course(Guid id,int number, string title,int credits, Guid departmentId)
        {
            ApplyChange(new CourseCreated(id, number, title, credits, departmentId));
            //Instructors = new List<Instructor>();
        }
        private void Apply(CourseCreated @event)
        {
            Id = @event.Id;
            Number = @event.Number;
            Title = @event.Title;
            Credits = @event.Credits;
            DepartmentId = @event.DepartmentId;
        }
        public void Update(string title,int credits,Guid departmentId)
        {
            ApplyChange(new CourseUpdated(Id, title, credits, departmentId));
        }
        private void Apply(CourseUpdated @event)
        {
            Id = @event.Id;
            Title = @event.Title;
            Credits = @event.Credits;
            DepartmentId = @event.DepartmentId;
        }

        public void Delete()
        {
            ApplyChange(new CourseDeleted(Id));
        }

        public int Number { get; private set; }
        
        public string Title { get; private set; }

        public int Credits { get; private set; }

        public Guid DepartmentId { get; private set; }

        //public Department Department { get; private set; }
        //public ICollection<Enrollment> Enrollments { get; set; }
        //public ICollection<Instructor> Instructors { get; set; }
    }
}
