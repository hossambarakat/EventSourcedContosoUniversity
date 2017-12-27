using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventSourcedContosoUniversity.Core.ReadModel.Students;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Students
{
    public class GetStudentDetailsQuery : IRequest<GetStudentDetailsQueryModel>
    {
        public Guid Id { get; set; }
    }
    public class GetStudentDetailsQueryValidator : AbstractValidator<GetStudentDetailsQuery>
    {
        public GetStudentDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class GetStudentDetailsQueryModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTimeOffset EnrollmentDate { get; set; }
    }

    public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, GetStudentDetailsQueryModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetStudentDetailsQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<GetStudentDetailsQueryModel> Handle(GetStudentDetailsQuery request, CancellationToken cancellationToken)
        {
            var model = await _readModelRepository.GetById<StudentReadModel>(request.Id);
            return new GetStudentDetailsQueryModel
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstMidName = model.FirstMidName,
                FullName = model.FullName,
                EnrollmentDate = model.EnrollmentDate
            };
        }
    }
}
