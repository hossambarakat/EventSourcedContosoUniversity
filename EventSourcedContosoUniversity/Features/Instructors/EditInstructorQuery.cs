using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class EditInstructorQuery : IRequest<EditInstructorCommand>
    {
        public Guid Id { get; set; }
    }
    public class EditInstructorQueryValidator : AbstractValidator<EditInstructorQuery>
    {
        public EditInstructorQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class EditInstructorQueryHandler : IRequestHandler<EditInstructorQuery, EditInstructorCommand>
    {
        private readonly IRepository<Instructor> _instructorsRepository;

        public EditInstructorQueryHandler(IRepository<Instructor> instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }
        public async Task<EditInstructorCommand> Handle(EditInstructorQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _instructorsRepository.GetById(request.Id);
            return new EditInstructorCommand
            {
                Id = instructor.Id,
                LastName = instructor.LastName,
                FirstName = instructor.FirstName,
                HireDate = instructor.HireDate,
                OfficeLocation = instructor.OfficeLocation,
                SelectedCourses = instructor.CourseAssignments.ToArray()
            };
        }
    }
    public class EditInstructorCommand : IRequest
    {
        public Guid Id { get; set; }
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
    public class EditInstructorCommandValidator : AbstractValidator<EditInstructorCommand>
    {
        public EditInstructorCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.HireDate).NotEmpty();
            RuleFor(x => x.OfficeLocation).MaximumLength(50);
        }
    }
    public class EditInstructorCommandHandler : AsyncRequestHandler<EditInstructorCommand>
    {
        private readonly IRepository<Instructor> _instructorsRepository;

        public EditInstructorCommandHandler(IRepository<Instructor> instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }
        protected override async Task Handle(EditInstructorCommand command, CancellationToken cancellationToken)
        {
            var instructor = await _instructorsRepository.GetById(command.Id);
            if(instructor == null)
            {
                throw new Exception("Instructor not found");
            }
            instructor.UpdateDetails(command.LastName, command.FirstName, command.HireDate);
            if (command.OfficeLocation != instructor.OfficeLocation)
            {
                instructor.AssignOfficeLocation(command.OfficeLocation);
            }
            instructor.UpdateAssignedCourses(command.SelectedCourses);

            await _instructorsRepository.Save(instructor);
        }
    }
}
