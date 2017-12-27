using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Courses
{
    public class EditCourseQuery : IRequest<EditCourseCommand>
    {
        public Guid Id { get; set; }
    }
    public class EditCourseQueryValidator : AbstractValidator<EditCourseQuery>
    {
        public EditCourseQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
   
    public class EditCourseQueryHandler : IRequestHandler<EditCourseQuery, EditCourseCommand>
    {
        private readonly IRepository<Course> _repository;

        public EditCourseQueryHandler(IRepository<Course> repository)
        {
            _repository = repository;
        }
        public async Task<EditCourseCommand> Handle(EditCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _repository.GetById(request.Id);
            return new EditCourseCommand
            {
                Id = course.Id,
                Number = course.Number,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = course.DepartmentId
            };
        }
    }

}
