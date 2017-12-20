using System;
using EventSourcedContosoUniversity.Core.Domain.Events;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Department : AggregateRoot
    {
        private Department() { }
        public Department(Guid id, string name, decimal budget, DateTimeOffset startDate, Guid? administartorId)
        {
            ApplyChange(new DepartmentCreated(id, name, budget, startDate, administartorId));
        }

        public void Apply(DepartmentCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Budget = @event.Budget;
            StartDate = @event.Startdate;
            AdministratorId = @event.AdministratorId;
        }

        public void UpdateDepartment(string name, decimal budget, DateTimeOffset startDate, Guid? administartorId)
        {
            ApplyChange(new DepartmentUpdated(Id, name, budget, startDate, administartorId));
        }

        public string Name { get; private set; }

        public decimal Budget { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public Guid? AdministratorId { get; private set; }
    }

}
