using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class EditDepartmentQuery : IRequest<EditDepartmentCommand>
    {
        public Guid Id { get; set; }
    }
    public class EditDepartmentQueryHandler : IRequestHandler<EditDepartmentQuery, EditDepartmentCommand>
    {
        private readonly IRepository<Department> _repository;

        public EditDepartmentQueryHandler(IRepository<Department> repository)
        {
            _repository = repository;
        }
        public async Task<EditDepartmentCommand> Handle(EditDepartmentQuery request, CancellationToken cancellationToken)
        {
            var department = await _repository.GetById(request.Id);
            var editDepartmentCommand = new EditDepartmentCommand
            {
                Id = department.Id,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                AdministratorId = department.AdministratorId
            };
            return editDepartmentCommand;
        }
    }
}
