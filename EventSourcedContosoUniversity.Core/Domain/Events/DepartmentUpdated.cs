using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class DepartmentUpdated : Event
    {
        public DepartmentUpdated(Guid id, string name, decimal budget, DateTimeOffset startdate)
        {
            Id = id;
            Name = name;
            Budget = budget;
            Startdate = startdate;
        }

        public Guid Id { get; }
        public string Name { get; }
        public decimal Budget { get; }
        public DateTimeOffset Startdate { get; }
    }
}
