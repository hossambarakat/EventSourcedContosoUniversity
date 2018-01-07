using System;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Domain.Exceptions;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Department : AggregateRoot
    {
        public string Name { get; private set; }
        public decimal Budget { get; private set; }
        public DateTimeOffset StartDate { get; private set; }
        public Guid? AdministratorId { get; private set; }

        private Department() { }
        public Department(Guid id, string name, decimal budget, DateTimeOffset startDate)
        {
            ApplyChange(new DepartmentCreated(id, name, budget, startDate));
        }

        public void Apply(DepartmentCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Budget = @event.Budget;
            StartDate = @event.Startdate;
        }

        public void Update(string name, decimal budget, DateTimeOffset startDate)
        {
            ApplyChange(new DepartmentUpdated(Id, name, budget, startDate));
        }

        public void Apply(DepartmentUpdated @event)
        {
            Name = @event.Name;
            Budget = @event.Budget;
            StartDate = @event.Startdate;
        }

        public void Delete()
        {
            ApplyChange(new DepartmentDeleted(Id));
        }

        public void AssignAdministrator(Guid administratorId)
        {
            ApplyChange(new DepartmentAdministratorAssigned(Id, administratorId));
        }
        public void Apply(DepartmentAdministratorAssigned @event)
        {
            AdministratorId = @event.AdministratorId;
        }
        public void UnassignAdministrator()
        {
            if(AdministratorId == null)
            {
                throw new DomainException("Failed to unassign administrator");
            }
            ApplyChange(new DepartmentAdministratorUnassigned(Id, AdministratorId.Value));
        }
        public void Apply(DepartmentAdministratorUnassigned @event)
        {
            AdministratorId = null;
        }
    }

}
