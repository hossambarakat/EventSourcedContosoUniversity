using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Courses
{
    public class CreateCourseCommand : IRequest
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        [DisplayName("Department")]
        public Guid DepartmentId { get; set; }
    }
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().Length(3, 50);
            RuleFor(x => x.Credits).InclusiveBetween(0, 5);
            RuleFor(x => x.DepartmentId).NotEmpty();
        }
    }
    public class CreateCourseCommandHandler : AsyncRequestHandler<CreateCourseCommand>
    {
        private readonly IRepository<Course> _repository;

        public CreateCourseCommandHandler(IRepository<Course> repository)
        {
            _repository = repository;
        }
        protected override Task Handle(CreateCourseCommand message, CancellationToken cancellationToken)
        {
            var course = new Course(Guid.NewGuid(), message.Number, message.Title, message.Credits, message.DepartmentId);
            return _repository.Save(course);
        }
    }
}
