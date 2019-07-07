using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class DeleteDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }
        public string Administrator { get; set; }
    }
    public class DeleteCommandHandler : AsyncRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IRepository<Department> _repository;

        public DeleteCommandHandler(IRepository<Department> repository)
        {
            _repository = repository;
        }
        protected override async Task Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _repository.GetById(command.Id);
            if(department==null)
            {
                throw new Exception("Department not found");
            }
            department.Delete();
            await _repository.Save(department);
        }
    }
}
