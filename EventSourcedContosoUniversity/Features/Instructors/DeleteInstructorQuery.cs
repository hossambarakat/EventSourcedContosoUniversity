using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Instructors;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class DeleteInstructorQuery : IRequest<DeleteInstructorQueryModel>
    {
        public Guid Id { get; set; }
    }
    public class DeleteInstructorQueryValidator : AbstractValidator<DeleteInstructorQuery>
    {
        public DeleteInstructorQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class DeleteInstructorQueryModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTimeOffset HireDate { get; set; }
    }
    public class DeleteInstructorQueryHandler : IRequestHandler<DeleteInstructorQuery, DeleteInstructorQueryModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public DeleteInstructorQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<DeleteInstructorQueryModel> Handle(DeleteInstructorQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _readModelRepository.GetById<InstructorReadModel>(request.Id);
            return new DeleteInstructorQueryModel
            {
                Id = instructor.Id,
                LastName = instructor.LastName,
                FirstName = instructor.FirstName,
                HireDate = instructor.HireDate
            };
        }
    }
}
