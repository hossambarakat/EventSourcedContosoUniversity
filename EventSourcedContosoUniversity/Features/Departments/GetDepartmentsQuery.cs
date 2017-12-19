using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using EventSourcedContosoUniversity.Core;
using EventSourcedContosoUniversity.Core.ReadModel;
using System.Linq;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class GetDepartmentsQuery : IRequest<List<GetDepartmentsQueryModel>>
    {
    }
    public class GetDepartmentsQueryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }
        public string Administrator { get; set; }
    }
    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<GetDepartmentsQueryModel>>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetDepartmentsQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }

        public async Task<List<GetDepartmentsQueryModel>> Handle(GetDepartmentsQuery message, CancellationToken cancellationToken)
        {
            var departments =  await _readModelRepository.All<DepartmentReadModel>();
            var model = departments.Select(x =>

                new GetDepartmentsQueryModel()
                {
                    Id = x.Id,
                    Administrator = x.Administrator,
                    Budget = x.Budget,
                    Name = x.Name,
                    StartDate = x.StartDate
                }
            );
            return model.ToList();
        }
    }
}
