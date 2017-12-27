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
    public class DeleteStudentQuery : IRequest<DeleteStudentQueryModel>
    {
        public Guid Id { get; set; }
    }
    public class DeleteStudentQueryValidator : AbstractValidator<DeleteStudentQuery>
    {
        public DeleteStudentQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class DeleteStudentQueryModel : IRequest
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
    public class DeleteStudentQueryHandler : IRequestHandler<DeleteStudentQuery, DeleteStudentQueryModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public DeleteStudentQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<DeleteStudentQueryModel> Handle(DeleteStudentQuery request, CancellationToken cancellationToken)
        {
            var studentReadModel = await _readModelRepository.GetById<StudentReadModel>(request.Id);
            return new DeleteStudentQueryModel
            {
                Id = studentReadModel.Id,
                LastName = studentReadModel.LastName,
                FirstMidName = studentReadModel.FirstMidName,
                FullName = studentReadModel.FullName,
                EnrollmentDate = studentReadModel.EnrollmentDate
            };
        }
    }
}
