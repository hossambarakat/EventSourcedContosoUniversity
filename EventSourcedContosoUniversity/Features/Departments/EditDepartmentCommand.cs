using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class EditDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }

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
    public class EditDepartmentCommandValidator : AbstractValidator<EditDepartmentCommand>
    {
        public EditDepartmentCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 50);
            RuleFor(x => x.Budget).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
        }
    }
    public class EditDepartmentCommandHandler : AsyncRequestHandler<EditDepartmentCommand>
    {
        private readonly IRepository<Department> _repository;

        public EditDepartmentCommandHandler(IRepository<Department> repository)
        {
            _repository = repository;
        }
        protected override async Task Handle(EditDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _repository.GetById(command.Id);
            if (department == null)
                throw new Exception("Entity not found");

            department.Update(command.Name, command.Budget, command.StartDate);
            if(department.AdministratorId != command.AdministratorId)
            {
                if(department.AdministratorId != null)
                {
                    department.UnassignAdministrator();
                }
                if (command.AdministratorId.HasValue)
                {
                    department.AssignAdministrator(command.AdministratorId.Value);
                }
            }
            await _repository.Save(department);
        }
    }

}
