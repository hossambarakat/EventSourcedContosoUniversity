using System;
using System.Collections.Generic;
using System.Linq;
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
                

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var viewModel = await _mediator.Send(new GetDepartmentsQuery());
            return View(viewModel);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(GetDepartmentDetailsQuery query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        // GET: Departments/Create
        public async Task<IActionResult> Create()
        {
            var instructorsReadModel = await _mediator.Send(new GetInstructorsQuery());
            ViewData["Administrators"] = new SelectList(instructorsReadModel, "Id", "FullName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget,StartDate,AdministratorId")] CreateDepartmentCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            var instructorsReadModel = await _mediator.Send(new GetInstructorsQuery());
            ViewData["Administrators"] = new SelectList(instructorsReadModel, "Id", "FullName");
            return View(command);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(EditDepartmentQuery query)
        {
            var department = await _mediator.Send(query);
            
            if (department == null)
            {
                return NotFound();
            }
            var instructorsReadModel = await _mediator.Send(new GetInstructorsQuery());
            ViewData["Administrators"] = new SelectList(instructorsReadModel, "Id", "FullName");
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDepartmentCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }

            var instructorsReadModel = await _mediator.Send(new GetInstructorsQuery());
            ViewData["Administrators"] = new SelectList(instructorsReadModel, "Id", "FullName");
            return View(command);
        }

        // GET: Departments/Delete/5
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

    }
}
