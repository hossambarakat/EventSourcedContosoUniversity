using System;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    [CollectionName("departments")]
    public class DepartmentReadModel : IMongoDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public string Administrator { get; set; }
    }
}
