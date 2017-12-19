using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    public class GetInstructorsQuery : IRequest<List<InstructorsReadModel>>
    {
    }
    public class InstructorsReadModel
    {
        public Guid Id { get; set; }
        public string FirstMidName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstMidName} {LastName}"; 
        public DateTimeOffset HireDate { get; set; }
        public string Location { get; set; }
    }
    public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, List<InstructorsReadModel>>
    {
        public Task<List<InstructorsReadModel>> Handle(GetInstructorsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<InstructorsReadModel>
            {
                new InstructorsReadModel
                {
                    Id = Guid.Parse("757b8311-a080-4cb5-ac09-09db8a752a93"),
                    FirstMidName = "Cool",
                    LastName = "Amazing"
                },
                new InstructorsReadModel
                {
                    Id = Guid.Parse("b177cff4-4318-468f-aebc-20afff2edb42 "),
                    FirstMidName = "Just",
                    LastName = "Amazing"
                }
            });
        }
    }
}
