using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class InstructorOfficeLocationChanged : Event
    {
        public InstructorOfficeLocationChanged(Guid id,string officeLocation)
        {
            Id = id;
            OfficeLocation = officeLocation;
        }

        public Guid Id { get; }
        public string OfficeLocation { get; }
    }
}
