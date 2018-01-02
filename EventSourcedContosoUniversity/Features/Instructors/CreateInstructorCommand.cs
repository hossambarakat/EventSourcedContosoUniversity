using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class AssignedCourseData
    {
        public Guid CourseID { get; set; }
        public int CourseNumber { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
    public class CreateInstructorCommand : IRequest
    {
        public CreateInstructorCommand()
        {
            SelectedCourses = new Guid[0];
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTimeOffset HireDate { get; set; }
        [Display(Name = "Office Location")]
        public string OfficeLocation { get; set; }
        public Guid[] SelectedCourses { get; set; }
    }

    public class CreateInstructorCommandValidator : AbstractValidator<CreateInstructorCommand>
    {
        public CreateInstructorCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.HireDate).NotEmpty();
            RuleFor(x => x.OfficeLocation).MaximumLength(50);
        }
    }
    public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand>
    {
        private readonly IRepository<Instructor> _repository;

        public CreateInstructorCommandHandler(IRepository<Instructor> repository)
        {
            _repository = repository;
        }
        public async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
        {
            var instructor = new Instructor(Guid.NewGuid(), command.LastName, command.FirstName, command.HireDate);
            if (command.OfficeLocation != null)
            {
                instructor.AssignOfficeLocation(command.OfficeLocation);
            }
            instructor.UpdateAssignedCourses(command.SelectedCourses);

            await _repository.Save(instructor);
        }
    }
}
