using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class EditDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTimeOffset StartDate { get; set; }

        [Display(Name = "Administrator")]
        public Guid? AdministratorId { get; set; }
    }
    public class EditDepartmentCommandHandler : IRequestHandler<EditDepartmentCommand>
    {
        public Task Handle(EditDepartmentCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
