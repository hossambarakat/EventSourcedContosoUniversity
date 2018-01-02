using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Instructors;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class GetInstructorDetailsQuery : IRequest<GetInstructorDetailsQueryModel>
    {
        public Guid Id { get; set; }
    }
    public class GetInstructorDetailsQueryValidator : AbstractValidator<GetInstructorDetailsQuery>
    {
        public GetInstructorDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class GetInstructorDetailsQueryModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTimeOffset HireDate { get; set; }
    }
    public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, GetInstructorDetailsQueryModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetInstructorDetailsQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<GetInstructorDetailsQueryModel> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _readModelRepository.GetById<InstructorReadModel>(request.Id);
            return new GetInstructorDetailsQueryModel
            {
                Id = instructor.Id,
                LastName = instructor.LastName,
                FirstName = instructor.FirstName,
                HireDate = instructor.HireDate
            };
        }
    }

}
