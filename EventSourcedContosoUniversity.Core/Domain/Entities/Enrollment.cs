using System;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment 
    {
        public Guid Id { get; protected set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        //[DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        //public Course Course { get; set; }
        //public Student Student { get; set; }
    }
}
