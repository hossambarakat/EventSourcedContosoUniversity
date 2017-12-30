using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Courses;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class CoursesProjectionActor : BaseProjectionActor<CourseReadModel>
    {
        private readonly IRepository<Department> _departmentRepository;

        public CoursesProjectionActor(EventStoreDispatcher dispatcher,
            IReadModelRepository readModelRepository,
            ICatchupPositionRepository catchupPositionRepository,
            IRepository<Department> departmentRepository)
            : base(dispatcher,readModelRepository,catchupPositionRepository)
        {
            _departmentRepository = departmentRepository;

            ReceiveAsync<CourseCreated>(async (c) =>
            {
                var department = await departmentRepository.GetById(c.DepartmentId);
                await readModelRepository.Add(new CourseReadModel
                {
                    Id = c.Id,
                    Number = c.Number,
                    Title = c.Title,
                    Credits = c.Credits,
                    Department = department.Name,
                    DepartmentId = c.DepartmentId
                });
            });

            ReceiveAsync<CourseUpdated>(async (c) =>
            {
                var course = await readModelRepository.GetById<CourseReadModel>(c.Id);
                if(course.DepartmentId!=c.DepartmentId)
                {
                    var department = await departmentRepository.GetById(c.DepartmentId);
                    course.Department = department.Name;
                }
                course.Title = c.Title;
                course.Credits = c.Credits;
                course.DepartmentId = c.DepartmentId;
                await readModelRepository.Update(course);
            });
            ReceiveAsync<CourseDeleted>(async (s) =>
            {
                var course = await readModelRepository.GetById<CourseReadModel>(s.Id);
                await readModelRepository.Delete<CourseReadModel>(course);
            });
            ReceiveAsync<DepartmentUpdated>(async (d) =>
            {
                var coursesCollection = readModelRepository.GetCollection<CourseReadModel>();
                await coursesCollection.UpdateManyAsync(x => x.DepartmentId == d.Id, Builders<CourseReadModel>.Update.Set(x => x.Department, d.Name));
            });
        }
    }
}
