using System;

namespace EventSourcedContosoUniversity.Core.Domain.Events
{
    public class InstructorCourseUnassigned : Event
    {
        public InstructorCourseUnassigned(Guid id, Guid courseId)
        {
            Id = id;
            CourseId = courseId;
        }

        public Guid Id { get; }
        public Guid CourseId { get; }
    }
}
