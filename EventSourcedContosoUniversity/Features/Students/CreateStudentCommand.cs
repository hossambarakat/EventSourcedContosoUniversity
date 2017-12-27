using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Students
{
    public class CreateStudentCommand : IRequest
    {
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTimeOffset EnrollmentDate { get; set; }
    }
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.FirstMidName).NotEmpty();
            RuleFor(x => x.FirstMidName).MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");
        }
    }

    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand>
    {
        private readonly IRepository<Student> _repository;

        public CreateStudentCommandHandler(IRepository<Student> repository)
        {
            _repository = repository;
        }
        public Task Handle(CreateStudentCommand message, CancellationToken cancellationToken)
        {
            var student = new Student(Guid.NewGuid(), message.FirstMidName, message.LastName, message.EnrollmentDate);
            return _repository.Save(student);
        }
    }
}
