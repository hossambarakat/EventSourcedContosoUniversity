using System;
using System.Collections.Generic;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Student : Person
    {
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Enrollment Date")]
        public DateTimeOffset EnrollmentDate { get; private set; }

        public ICollection<Enrollment> Enrollments { get; private set; }
    }
}
