using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class DeleteInstructorCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteInstructorCommandValidator : AbstractValidator<DeleteInstructorCommand>
    {
        public DeleteInstructorCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand>
    {
        private readonly IRepository<Instructor> _instructorsRepository;

        public DeleteInstructorCommandHandler(IRepository<Instructor> instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }

        public async Task Handle(DeleteInstructorCommand command, CancellationToken cancellationToken)
        {
            var instructor = await _instructorsRepository.GetById(command.Id);
            instructor.Delete();
            await _instructorsRepository.Save(instructor);
        }
    }
}
