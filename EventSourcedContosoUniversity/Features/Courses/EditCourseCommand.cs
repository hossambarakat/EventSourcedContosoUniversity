using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Courses
{
    public class EditCourseCommand : IRequest
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
    }
    public class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
    {
        public EditCourseCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.Title).Length(3, 50);
            RuleFor(x => x.Credits).InclusiveBetween(0, 5);
            RuleFor(x => x.DepartmentId).NotEmpty();
        }
    }
    public class EditCourseCommandHandler : IRequestHandler<EditCourseCommand>
    {
        private readonly IRepository<Course> _repository;

        public EditCourseCommandHandler(IRepository<Course> repository)
        {
            _repository = repository;
        }
        public async Task Handle(EditCourseCommand message, CancellationToken cancellationToken)
        {
            var course = await _repository.GetById(message.Id);
            //TODO: handle not found
            course.Update(message.Title, message.Credits, message.DepartmentId);
            await _repository.Save(course);
        }
    }
}
