using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Courses;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventStore.ClientAPI;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class CoursesProjectionActor : BaseProjectionActor<CourseReadModel>
    {
        readonly EventStoreDispatcher _dispatcher;
        private readonly ICatchupPositionRepository _catchupPositionRepository;

        public CoursesProjectionActor(EventStoreDispatcher dispatcher, IReadModelRepository repository, ICatchupPositionRepository catchupPositionRepository)
            : base(dispatcher,repository,catchupPositionRepository)
        {
            _catchupPositionRepository = catchupPositionRepository;
            _dispatcher = dispatcher;
            ReceiveAsync<CourseCreated>(async (c) =>
            {
                await repository.Add(new CourseReadModel
                {
                    Id = c.Id,
                    Number = c.Number,
                    Title = c.Title,
                    Credits = c.Credits,
                    Department = c.DepartmentId.ToString(),
                    DepartmentId = c.DepartmentId
                });
            });

            ReceiveAsync<CourseUpdated>(async (c) =>
            {
                var course = await repository.GetById<CourseReadModel>(c.Id);
                course.Title = c.Title;
                course.Credits = c.Credits;
                course.DepartmentId = c.DepartmentId;
                course.Department = c.DepartmentId.ToString();
                await repository.Update(course);
            });
            ReceiveAsync<CourseDeleted>(async (s) =>
            {
                var course = await repository.GetById<CourseReadModel>(s.Id);
                await repository.Delete<CourseReadModel>(course);
            });
            ReceiveAsync<SaveEventMessage>( async (s) =>
            {
                await _catchupPositionRepository.SavePosition<DepartmentReadModel>(new Position(s.CommitPosition, s.PreparePosition));
            });
        }
    }
}
