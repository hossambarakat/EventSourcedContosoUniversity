using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Core.Domain.Repositories;
using EventSourcedContosoUniversity.Core.ReadModel.Courses;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Courses
{
    public class DeleteCourseQuery : IRequest<DeleteCourseQueryModel>
    {
        public Guid Id { get; set; }
    }
    public class DeleteCourseQueryValidator : AbstractValidator<DeleteCourseQuery>
    {
        public DeleteCourseQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class DeleteCourseQueryModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
    }
    public class DeleteCourseQueryHandler : IRequestHandler<DeleteCourseQuery, DeleteCourseQueryModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public DeleteCourseQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        
        public async Task<DeleteCourseQueryModel> Handle(DeleteCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _readModelRepository.GetById<CourseReadModel>(request.Id);
            return new DeleteCourseQueryModel
            {
                Id = course.Id,
                Number = course.Number,
                Title = course.Title,
                Credits = course.Credits,
                Department = course.Department
            };
        }
    }

    public class DeleteCourseCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseQuery>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
    {
        private readonly IRepository<Course> _repository;

        public DeleteCourseCommandHandler(IRepository<Course> repository)
        {
            _repository = repository;
        }
        public async Task Handle(DeleteCourseCommand message, CancellationToken cancellationToken)
        {
            var course = await _repository.GetById(message.Id);
            course.Delete();
            await _repository.Save(course);
        }
    }
}
