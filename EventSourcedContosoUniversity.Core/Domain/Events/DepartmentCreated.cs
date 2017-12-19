using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class DepartmentCreated : Event
    {
        public DepartmentCreated(Guid id,string name, decimal budget, DateTimeOffset startdate,Guid? administratorId)
        {
            Id = id;
            Name = name;
            Budget = budget;
            Startdate = startdate;
            AdministratorId = administratorId;
        }

        public Guid Id { get; }
        public string Name { get; }
        public decimal Budget { get; }
        public DateTimeOffset Startdate { get; }
        public Guid? AdministratorId { get; }
    }
}
