using System;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSourcedContosoUniversity.Core.ReadModel.Students
{
    [CollectionName("Students")]
    public class StudentReadModel : IMongoDocument
    {
        public Guid Id { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
        public DateTimeOffset EnrollmentDate { get; set; }
    }
}
