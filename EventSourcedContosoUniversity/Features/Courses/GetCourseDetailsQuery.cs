using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Courses;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using FluentValidation;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Courses
{
    public class GetCourseDetailsQuery : IRequest<GetCourseDetailsModel>
    {
        public Guid Id { get; set; }
    }
    public class GetCourseDetailsValidator : AbstractValidator<GetCourseDetailsQuery>
    {
        public GetCourseDetailsValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class GetCourseDetailsModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
    }
    public class GetCourseDetailsHandler : IRequestHandler<GetCourseDetailsQuery, GetCourseDetailsModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetCourseDetailsHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<GetCourseDetailsModel> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
        {
            var course = await _readModelRepository.GetById<CourseReadModel>(request.Id);
            return new GetCourseDetailsModel
            {
                Id = course.Id,
                Number = course.Number,
                Title = course.Title,
                Credits = course.Credits,
                Department = course.Department
            };
        }
    }
}
