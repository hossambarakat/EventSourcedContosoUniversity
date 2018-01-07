using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;

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
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 50);
            RuleFor(x => x.Budget).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
        }
    }
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand>
    {
        private readonly IRepository<Department> _repository;

        public CreateDepartmentCommandHandler(IRepository<Department> repository)
        {
            _repository = repository;
        }
        public Task Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = new Department(Guid.NewGuid(), command.Name, command.Budget, command.StartDate);
            if(command.AdministratorId.HasValue)
            {
                department.AssignAdministrator(command.AdministratorId.Value);
            }
            return _repository.Save(department);
        }
    }
}
