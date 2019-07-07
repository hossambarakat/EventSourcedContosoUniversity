using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Students
{
    public class DeleteStudentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class DeleteStudentCommandHandler : AsyncRequestHandler<DeleteStudentCommand>
    {
        private readonly IRepository<Student> _repository;

        public DeleteStudentCommandHandler(IRepository<Student> repository)
        {
            _repository = repository;
        }
        protected override async Task Handle(DeleteStudentCommand message, CancellationToken cancellationToken)
        {
            var student = await _repository.GetById(message.Id);
            student.Delete();
            await _repository.Save(student);
        }
    }
}
