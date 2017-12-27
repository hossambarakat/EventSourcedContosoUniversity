using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventSourcedContosoUniversity.Core.ReadModel.Students;
using MediatR;
using X.PagedList;

namespace EventSourcedContosoUniversity.Features.Students
{
    public class GetStudentsQuery : IRequest<GetStudentsQueryResult>
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? Page { get; set; }
    }
    public class GetStudentsQueryResult
    {
        public string CurrentSort { get; set; }
        public string NameSortParm { get; set; }
        public string DateSortParm { get; set; }
        public string CurrentFilter { get; set; }
        public IPagedList<GetStudentsQueryModel> Students { get; set; }
    }
    public class GetStudentsQueryModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTimeOffset EnrollmentDate { get; set; }
    }
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsQueryResult>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetStudentsQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }
        public Task<GetStudentsQueryResult> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var currentSort = request.SortOrder;
            var nameSortParm = string.IsNullOrEmpty(request.SortOrder) ? "name_desc" : "";
            var dateSortParm = request.SortOrder == "Date" ? "date_desc" : "Date";

            if (request.SearchString != null)
            {
                request.Page = 1;
            }
            else
            {
                request.SearchString = request.CurrentFilter;
            }

            var currentFilter = request.SearchString;

            var students = _readModelRepository.AllAsQueryable<StudentReadModel>();
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                students = students.Where(s => s.LastName.Contains(request.SearchString)
                                       || s.FirstMidName.Contains(request.SearchString));
            }
            switch (request.SortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            var studentsModel = students.ToList().Select(s => new GetStudentsQueryModel
            {
                Id = s.Id,
                LastName = s.LastName,
                FirstMidName = s.FirstMidName,
                FullName = s.FullName,
                EnrollmentDate = s.EnrollmentDate
            });
            int pageSize = 3;
            return Task.FromResult(new GetStudentsQueryResult
            {
                CurrentSort = currentSort,
                NameSortParm = nameSortParm,
                DateSortParm = dateSortParm,
                CurrentFilter = currentFilter,
                Students = studentsModel.ToPagedList(request.Page.HasValue ? request.Page.Value : 1, pageSize)
            }
            );
        }
    }
}
