using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Courses;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using MediatR;


namespace EventSourcedContosoUniversity.Features.Courses
{
    public class GetCoursesQuery : IRequest<List<GetCoursesQueryModel>>
    {
    }
    public class GetCoursesQueryModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
    }
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<GetCoursesQueryModel>>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetCoursesQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<List<GetCoursesQueryModel>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _readModelRepository.All<CourseReadModel>();

            return courses.Select(x => new GetCoursesQueryModel
            {
                Id = x.Id,
                Number=x.Number,
                Title = x.Title,
                Credits = x.Credits,
                Department = x.Department
            }).ToList();
        }
    }
}
