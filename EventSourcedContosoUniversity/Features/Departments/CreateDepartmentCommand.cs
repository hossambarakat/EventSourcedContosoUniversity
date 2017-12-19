using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using EventSourcedContosoUniversity.Core.Domain;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class CreateDepartmentCommand : IRequest
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTimeOffset StartDate { get; set; }

        [Display(Name = "Administrator")]
        public Guid? AdministratorId { get; set; }
    }
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand>
    {
        private readonly IRepository<Department> _repository;

        public CreateDepartmentCommandHandler(IRepository<Department> repository)
        {
            _repository = repository;
        }
        public Task Handle(CreateDepartmentCommand message, CancellationToken cancellationToken)
        {
            var department = new Department(Guid.NewGuid(), message.Name, message.Budget, message.StartDate, message.AdministratorId);
            return _repository.Save(department);
        }
    }
}
