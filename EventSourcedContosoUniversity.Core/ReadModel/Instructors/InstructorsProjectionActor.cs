using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel.Instructors
{
    public class InstructorsProjectionActor : BaseProjectionActor<InstructorReadModel>
    {
        public InstructorsProjectionActor(EventStoreDispatcher dispatcher,
            IReadModelRepository readModelRepository,
            ICatchupPositionRepository catchupPositionRepository, IRepository<Course> courseRepository)
            : base(dispatcher, readModelRepository, catchupPositionRepository)
        {
            ReceiveAsync<InstructorCreated>(async (i) =>
            {
                var instructorReadModel = new InstructorReadModel
                {
                    Id = i.Id,
                    LastName = i.LastName,
                    FirstName = i.FirstName,
                    HireDate = i.HireDate,
                };
                await readModelRepository.Add(instructorReadModel);
            });
            ReceiveAsync<InstructorDetailsUpdated>(async (i) =>
            {
                var instructorReadModel = await readModelRepository.GetById<InstructorReadModel>(i.Id);
                instructorReadModel.LastName = i.LastName;
                instructorReadModel.FirstName = i.FirstName;
                instructorReadModel.HireDate = i.HireDate;
                await readModelRepository.Update(instructorReadModel);
            });
            ReceiveAsync<InstructorOfficeLocationChanged>(async (o) =>
            {
                var instructor = await readModelRepository.GetById<InstructorReadModel>(o.Id);
                instructor.OfficeLocation = o.OfficeLocation;
                await readModelRepository.Update(instructor);
            });

            ReceiveAsync<InstructorCourseAssigned>(async (c) =>
            {
                var course = await courseRepository.GetById(c.CourseId);
                var instructorReadModel = await readModelRepository.GetById<InstructorReadModel>(c.Id);
                instructorReadModel.CourseAssignments.Add(new AssignedCourse
                {
                    Id = c.CourseId,
                    Title = course.Title,
                    Number = course.Number
                });
                await readModelRepository.Update(instructorReadModel);

            });
            ReceiveAsync<InstructorCourseUnassigned>(async (c) =>
            {
                var instructorReadModel = await readModelRepository.GetById<InstructorReadModel>(c.Id);
                instructorReadModel.CourseAssignments.RemoveAll(x => x.Id == c.CourseId);
                await readModelRepository.Update(instructorReadModel);
            });
            ReceiveAsync<InstructorDeleted>(async (i) =>
            {
                var instructorReadModel = await readModelRepository.GetById<InstructorReadModel>(i.Id);
                await readModelRepository.Delete(instructorReadModel);
            });
        }
    }
}
