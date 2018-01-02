using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcedContosoUniversity.Core.Domain.Events;

namespace EventSourcedContosoUniversity.Core.Domain.Entities
{
    public class Instructor : AggregateRoot
    {
        private Instructor() { }
        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public DateTimeOffset HireDate { get; private set; }

        public string OfficeLocation { get; private set; }

        public List<Guid> CourseAssignments { get; private set; }

        public Instructor(Guid id,string lastName, string firstName,DateTimeOffset hireDate)
        {
            ApplyChange(new InstructorCreated(id, lastName, firstName, hireDate));
        }
        public void Apply(InstructorCreated @event)
        {
            Id = @event.Id;
            LastName = @event.LastName;
            FirstName = @event.FirstName;
            HireDate = @event.HireDate;
            CourseAssignments = new List<Guid>();
        }
        public void AssignOfficeLocation(string officeLocation)
        {
            if(officeLocation != OfficeLocation)
            {
                ApplyChange(new InstructorOfficeLocationChanged(Id, officeLocation));
            }
        }
        private void Apply(InstructorOfficeLocationChanged @event)
        {
            OfficeLocation = @event.OfficeLocation;
        }
        public void UpdateAssignedCourses(IList<Guid> selectedCourses)
        {
            if (selectedCourses == null)
            {
                CourseAssignments?.ToList().ForEach(x=> UnassignCourse(x)) ;
                return;
            }

            var addedCourses = selectedCourses.Except(CourseAssignments);
            addedCourses.ToList().ForEach(x => AssignCourse(x));

            var removedCourses = CourseAssignments.Except(selectedCourses);
            removedCourses.ToList().ForEach(x => UnassignCourse(x));
        }

        public void AssignCourse(Guid courseId)
        {
            ApplyChange(new InstructorCourseAssigned(Id, courseId));
        }
        private void Apply(InstructorCourseAssigned @event)
        {
            CourseAssignments.Add(@event.CourseId);
        }
        public void UnassignCourse(Guid courseId)
        {
            ApplyChange(new InstructorCourseUnassigned(Id, courseId));
        }
        private void Apply(InstructorCourseUnassigned @event)
        {
            CourseAssignments.Remove(@event.CourseId);
        }
        public void UpdateDetails(string lastName,string firstName, DateTimeOffset hireDate)
        {
            ApplyChange(new InstructorDetailsUpdated(Id, lastName, firstName, hireDate));
        }
        private void Apply(InstructorDetailsUpdated @event)
        {
            Id = @event.Id;
            LastName = @event.LastName;
            FirstName = @event.FirstName;
            HireDate = @event.HireDate;
        }
        public void Delete()
        {
            ApplyChange(new InstructorDeleted(Id));
        }
    }
}
