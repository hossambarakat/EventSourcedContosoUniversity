using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Students
{
    public class EditStudentQuery : IRequest<EditStudentCommand>
    {
        public Guid Id { get; set; }
    }
    public class EditStudentQueryValidator : AbstractValidator<EditStudentQuery>
    {
        public EditStudentQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class EditStudentQueryHandler : IRequestHandler<EditStudentQuery, EditStudentCommand>
    {
        private readonly IRepository<Student> _repository;

        public EditStudentQueryHandler(IRepository<Student> repository)
        {
            _repository = repository;
        }

        public async Task<EditStudentCommand> Handle(EditStudentQuery request, CancellationToken cancellationToken)
        {
            var student = await _repository.GetById(request.Id);
            return new EditStudentCommand
            {
                Id = student.Id,
                FirstMidName = student.FirstMidName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate
            };
        }
    }

}
