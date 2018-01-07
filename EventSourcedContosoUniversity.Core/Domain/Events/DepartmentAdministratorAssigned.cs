using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class DepartmentAdministratorAssigned : Event
    {
        public DepartmentAdministratorAssigned(Guid departmentId, Guid administratorId)
        {
            DepartmentId = departmentId;
            AdministratorId = administratorId;
        }

        public Guid DepartmentId { get; }
        public Guid AdministratorId { get; }
    }
}
