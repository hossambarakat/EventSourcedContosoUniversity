using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core;
using EventSourcedContosoUniversity.Core.ReadModel;
using EventSourcedContosoUniversity.Core.ReadModel.Departments;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class DeleteDepartmentQuery : IRequest<DeleteDepartmentCommand>
    {
        public Guid Id { get; set; }
    }

    public class DeleteQueryHandler : IRequestHandler<DeleteDepartmentQuery, DeleteDepartmentCommand>
    {
        private readonly IReadModelRepository _readModelRepository;

        public DeleteQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }

        public async Task<DeleteDepartmentCommand> Handle(DeleteDepartmentQuery request, CancellationToken cancellationToken)
        {
            var department = await _readModelRepository.GetById<DepartmentReadModel>(request.Id);
            var model = new DeleteDepartmentCommand
            {
                Id = department.Id,
                Administrator = department.Administrator,
                Budget = department.Budget,
                Name = department.Name,
                StartDate = department.StartDate
            };
            return model;
        }
    }

    public class DeleteDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }
        public string Administrator { get; set; }
    }
    public class DeleteCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        public Task Handle(DeleteDepartmentCommand message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
