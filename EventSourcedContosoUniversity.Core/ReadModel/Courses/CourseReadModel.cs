using System;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel.Courses
{
    [CollectionName("Courses")]
    public class CourseReadModel : IMongoDocument
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
        public string Department { get; set; }
    }
}
