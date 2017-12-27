using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class StudentCreated : Event
    {
        public Guid Id { get; private set; }
        public string LastName { get; private set; }
        public string FirstMidName { get; private set; }
        public DateTimeOffset EnrollmentDate { get; private set; }
        public StudentCreated(Guid id, string firstMidName, string lastName, DateTimeOffset enrollmentDate)
        {
            Id = id;
            FirstMidName = firstMidName;
            LastName = lastName;
            EnrollmentDate = enrollmentDate;
        }
    }
}
