using System;
using EventSourcedContosoUniversity.Core.Domain.Events;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Student : AggregateRoot
    {
        public string LastName { get; private set; }

        public string FirstMidName { get; private set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
        
        public DateTimeOffset EnrollmentDate { get; private set; }

        public Student(Guid id, string firstMidName, string lastName, DateTimeOffset enrollmentDate)
        {
            ApplyChange(new StudentCreated(id, firstMidName, lastName, enrollmentDate));
        }
        private Student() { }

        public void Apply(StudentCreated @event)
        {
            Id = @event.Id;
            FirstMidName = @event.FirstMidName;
            LastName = @event.LastName;
            EnrollmentDate = @event.EnrollmentDate;
        }

        public void Update(string firstMidName, string lastName, DateTimeOffset enrollmentDate)
        {
            ApplyChange(new StudentUpdated(Id, firstMidName, lastName, enrollmentDate));
        }

        public void Apply(StudentUpdated @event)
        {
            FirstMidName = @event.FirstMidName;
            LastName = @event.LastName;
            EnrollmentDate = @event.EnrollmentDate;
        }
        public void Delete()
        {
            ApplyChange(new StudentDeleted(Id));
        }
    }
}
