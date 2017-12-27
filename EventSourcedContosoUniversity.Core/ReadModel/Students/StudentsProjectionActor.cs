using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventSourcedContosoUniversity.Core.ReadModel.Students;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class StudentsProjectionActor : BaseProjectionActor<StudentReadModel>
    {
        private readonly ICatchupPositionRepository _catchupPositionRepository;

        public StudentsProjectionActor(EventStoreDispatcher dispatcher, IReadModelRepository repository, ICatchupPositionRepository catchupPositionRepository)
            : base(dispatcher,repository,catchupPositionRepository)
        {
            _catchupPositionRepository = catchupPositionRepository;
            
            ReceiveAsync<StudentCreated>(async (s) =>
            {
                await repository.Add(new StudentReadModel
                {
                    Id = s.Id,
                    LastName = s.LastName,
                    FirstMidName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate
                });
            });

            ReceiveAsync<StudentUpdated>(async (s) =>
            {
                var student = await repository.GetById<StudentReadModel>(s.Id);
                student.FirstMidName = s.FirstMidName;
                student.LastName = s.LastName;
                student.EnrollmentDate = s.EnrollmentDate;
                await repository.Update(student);
            });
            ReceiveAsync<StudentDeleted>(async (s) =>
            {
                var student = await repository.GetById<StudentReadModel>(s.Id);
                await repository.Delete(student);
            });
        }

    }
}
