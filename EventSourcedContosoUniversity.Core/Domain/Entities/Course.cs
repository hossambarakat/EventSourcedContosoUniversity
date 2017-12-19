using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcedContosoUniversity.Core.Domain
{
    public class Course 
    {
        public Course()
        {
            Instructors = new List<Instructor>();
        }
        //[Display(Name = "Number")]
        public Guid Id { get; protected set; }

        //[StringLength(50, MinimumLength = 3)]
        public string Title { get; private set; }

        //[Range(0, 5)]
        public int Credits { get; private set; }

        public int DepartmentID { get; private set; }

        public Department Department { get; private set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
