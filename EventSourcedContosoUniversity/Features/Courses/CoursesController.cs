using System.Threading.Tasks;
using EventSourcedContosoUniversity.Features.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventSourcedContosoUniversity.Features.Courses
{
    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _mediator.Send(new GetCoursesQuery());
            return View(courses);
        }

        public async Task<IActionResult> Details(GetCourseDetailsQuery query)
        {
            var course = await _mediator.Send(query);

            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseCommand command)
        {
            //TODO: handle failure
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(EditCourseQuery query)
        {
            var course = await _mediator.Send(query);
            await PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EditCourseCommand command)
        {
            //TODO: handle failure
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departments = await _mediator.Send(new GetDepartmentsQuery());
            ViewBag.DepartmentID = new SelectList(departments, "Id", "Name", selectedDepartment);
        }

        public async Task<IActionResult> Delete(DeleteCourseQuery query)
        {
            var course = await _mediator.Send(query);
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteCourseCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
