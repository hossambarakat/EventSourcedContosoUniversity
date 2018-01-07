using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class DepartmentAdministratorUnassigned : Event
    {
        public DepartmentAdministratorUnassigned(Guid departmentId, Guid administratorId)
        {
            DepartmentId = departmentId;
            AdministratorId = administratorId;
        }

        public Guid DepartmentId { get; }
        public Guid AdministratorId { get; }
    }
}
