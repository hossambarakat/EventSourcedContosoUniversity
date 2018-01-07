using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventSourcedContosoUniversity.Features.Instructors;

namespace EventSourcedContosoUniversity.Features.Departments
{
    public class DepartmentsController : Controller
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _mediator.Send(new GetDepartmentsQuery());
            return View(viewModel);
        }

        public async Task<IActionResult> Details(GetDepartmentDetailsQuery query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateInstructorsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget,StartDate,AdministratorId")] CreateDepartmentCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            await PopulateInstructorsDropDownList();
            return View(command);
        }

        public async Task<IActionResult> Edit(EditDepartmentQuery query)
        {
            var department = await _mediator.Send(query);
            
            if (department == null)
            {
                return NotFound();
            }
            await PopulateInstructorsDropDownList();
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDepartmentCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }

            await PopulateInstructorsDropDownList();
            return View(command);
        }

        public async Task<IActionResult> Delete(DeleteDepartmentQuery query)
        {
            var command = await _mediator.Send(query);

            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteDepartmentCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateInstructorsDropDownList()
        {
            var instructorsReadModel = await _mediator.Send(new GetInstructorsQuery());
            ViewData["Administrators"] = new SelectList(instructorsReadModel, "Id", "FullName");
        }

    }
}
