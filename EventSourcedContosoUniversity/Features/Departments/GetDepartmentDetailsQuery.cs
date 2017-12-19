using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core;
using EventSourcedContosoUniversity.Core.ReadModel;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class GetDepartmentDetailsQuery : IRequest<GetDepartmentDetailsQueryModel>
    {
        public Guid Id { get; set; }
    }
    public class GetDepartmentDetailsQueryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }
        public string Administrator { get; set; }
    }
    public class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, GetDepartmentDetailsQueryModel>
    {
        private readonly IReadModelRepository _readModelRepository;

        public GetDepartmentDetailsQueryHandler(IReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }

        public async Task<GetDepartmentDetailsQueryModel> Handle(GetDepartmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var department = await _readModelRepository.GetById<DepartmentReadModel>(request.Id);
            var model = new GetDepartmentDetailsQueryModel
            {
                Id = department.Id,
                Administrator = department.Administrator,
                Budget = department.Budget,
                Name = department.Name,
                StartDate = department.StartDate
            };
            return model;
        }
    }
}
