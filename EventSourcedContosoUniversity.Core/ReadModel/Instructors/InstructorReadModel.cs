using System;
using System.Collections.Generic;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel.Instructors
{
    [CollectionName("Instructors")]
    public class InstructorReadModel : IMongoDocument
    {
        public InstructorReadModel()
        {
            CourseAssignments = new List<AssignedCourse>();
        }
        public Guid Id { get; set; }
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public DateTimeOffset HireDate { get; set; }

        public string OfficeLocation { get; set; }

        public List<AssignedCourse> CourseAssignments { get; set; }
    }
    public class AssignedCourse
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
    }
}
