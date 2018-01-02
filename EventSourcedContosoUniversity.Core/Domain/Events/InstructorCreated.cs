using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class InstructorCreated : Event
    {
        public InstructorCreated(Guid id,string lastName,string firstName,DateTimeOffset hireDate)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            HireDate = hireDate;
        }
        public Guid Id { get; private set; }
        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public DateTimeOffset HireDate { get; private set; }
    }
}
