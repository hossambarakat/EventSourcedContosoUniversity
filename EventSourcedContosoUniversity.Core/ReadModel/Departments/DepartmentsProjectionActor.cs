using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class DepartmentsProjectionActor : BaseProjectionActor<DepartmentReadModel>
    {
        public DepartmentsProjectionActor(EventStoreDispatcher dispatcher, IReadModelRepository repository, ICatchupPositionRepository catchupPositionRepository)
            : base(dispatcher, repository, catchupPositionRepository)
        {
            ReceiveAsync<DepartmentCreated>(async (s) =>
            {
                await repository.Add(new DepartmentReadModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Budget = s.Budget,
                    Administrator = s.AdministratorId.ToString(),
                    StartDate = s.Startdate
                });
            });

            ReceiveAsync<DepartmentUpdated>(async (s) =>
            {
                var department = await repository.GetById<DepartmentReadModel>(s.Id);
                department.Name = s.Name;
                department.Budget = s.Budget;
                department.Administrator = s.AdministratorId.ToString();
                department.StartDate = s.Startdate;
                await repository.Update(department);
            });
            ReceiveAsync<DepartmentDeleted>(async (s) =>
            {
                var department = await repository.GetById<DepartmentReadModel>(s.Id);
                await repository.Delete<DepartmentReadModel>(department);
            });
        }
    }
}
