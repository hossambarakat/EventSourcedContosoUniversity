using System;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class DepartmentsProjectionActor : BaseProjectionActor<DepartmentReadModel>
    {
        private readonly IRepository<Instructor> _instructorRepository;

        public DepartmentsProjectionActor(EventStoreDispatcher dispatcher,
            IReadModelRepository readModelRepository,
            ICatchupPositionRepository catchupPositionRepository,
            IRepository<Instructor> instructorRepository)
            : base(dispatcher, readModelRepository, catchupPositionRepository)
        {
            _instructorRepository = instructorRepository;
            ReceiveAsync<DepartmentCreated>(async (s) =>
            {
                await readModelRepository.Add(new DepartmentReadModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Budget = s.Budget,
                    StartDate = s.Startdate
                });
            });

            ReceiveAsync<DepartmentUpdated>(async (s) =>
            {
                var department = await readModelRepository.GetById<DepartmentReadModel>(s.Id);
                department.Name = s.Name;
                department.Budget = s.Budget;
                department.StartDate = s.Startdate;
                await readModelRepository.Update(department);
            });
            ReceiveAsync<DepartmentDeleted>(async (s) =>
            {
                var department = await readModelRepository.GetById<DepartmentReadModel>(s.Id);
                await readModelRepository.Delete<DepartmentReadModel>(department);
            });
            ReceiveAsync<DepartmentAdministratorAssigned>(async (@event) =>
            {
                var instructorName = await GetInstructorName(@event.AdministratorId);
                var department = await readModelRepository.GetById<DepartmentReadModel>(@event.DepartmentId);
                department.Administrator = instructorName;
                await readModelRepository.Update(department);
            });
            ReceiveAsync<DepartmentAdministratorUnassigned>(async (@event) =>
            {
                var department = await readModelRepository.GetById<DepartmentReadModel>(@event.DepartmentId);
                department.Administrator = null;
                await readModelRepository.Update(department);
            });
        }

        private async Task<string> GetInstructorName(Guid administratorId)
        {
            string instructorName = null;
            var instructor = await _instructorRepository.GetById(administratorId);
            instructorName = instructor.FullName;

            return instructorName;
        }
    }
}
