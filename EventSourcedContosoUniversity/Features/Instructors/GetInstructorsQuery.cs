using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Instructors;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using MediatR;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class GetInstructorsQuery : IRequest<List<GetInstructorsQueryModel>>
    {
    }
    public class GetInstructorsQueryModel
    {
        public GetInstructorsQueryModel()
        {
            CourseAssignments = new List<AssignedCourseData>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTimeOffset HireDate { get; set; }
        [Display(Name = "Office Location")]
        public string OfficeLocation { get; set; }
        public List<AssignedCourseData> CourseAssignments { get; set; }
    }
    public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, List<GetInstructorsQueryModel>>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetInstructorsQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public async Task<List<GetInstructorsQueryModel>> Handle(GetInstructorsQuery request, CancellationToken cancellationToken)
        {
            var instructors = await _readModelRepository.All<InstructorReadModel>();
            var model = instructors.Select(x => new GetInstructorsQueryModel
            {
                Id = x.Id,
                LastName = x.LastName,
                FirstName = x.FirstName,
                HireDate = x.HireDate,
                OfficeLocation = x.OfficeLocation,
                CourseAssignments = x.CourseAssignments.Select(c => new AssignedCourseData
                {
                    Assigned = true,
                    CourseID = c.Id,
                    CourseNumber = c.Number,
                    Title = c.Title
                }).ToList()
            }).ToList();
            return model;
        }
    }
}
