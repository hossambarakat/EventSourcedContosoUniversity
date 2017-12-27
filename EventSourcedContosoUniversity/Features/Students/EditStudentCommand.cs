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
    public class EditStudentCommand : IRequest
    {
        public Guid Id { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTimeOffset EnrollmentDate { get; set; }
    }
    public class EditStudentCommandValidator : AbstractValidator<EditStudentCommand>
    {
        public EditStudentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.FirstMidName).NotEmpty();
            RuleFor(x => x.FirstMidName).MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");
        }
    }
    public class EditStudentCommandHandler : IRequestHandler<EditStudentCommand>
    {
        private readonly IRepository<Student> _repository;

        public EditStudentCommandHandler(IRepository<Student> repository)
        {
            _repository = repository;
        }
        public async Task Handle(EditStudentCommand message, CancellationToken cancellationToken)
        {
            var student = await _repository.GetById(message.Id);
            student.Update(message.FirstMidName, message.LastName, message.EnrollmentDate);
            await _repository.Save(student);
        }
    }
}
