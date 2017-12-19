using System;

namespace EventSourcedContosoUniversity.Core.Domain
{
    public class Instructor : Person
    {
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Hire Date")]
        public DateTimeOffset HireDate { get; set; }

        //public ICollection<CourseAssignment> CourseAssignments { get; set; }
        //public OfficeAssignment OfficeAssignment { get; set; }

        //[StringLength(50)]
        //[Display(Name = "Office Location")]
        public string Location { get; set; }
    }
}
