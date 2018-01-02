using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class InstructorCourseAssigned : Event
    {
        public InstructorCourseAssigned(Guid id, Guid courseId)
        {
            Id = id;
            CourseId = courseId;
        }

        public Guid Id { get; }
        public Guid CourseId { get; }
    }
}
