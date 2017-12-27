using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcedContosoUniversity.Features.Students
{

    public class StudentsController : Controller
    {
        private IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Students
        public async Task<IActionResult> Index(GetStudentsQuery query)
        {
            var result = await _mediator.Send(query);
            return View(result);
        }
        // GET: Students/Details/5
        public async Task<IActionResult> Details(GetStudentDetailsQuery query)
        {
            //TODO: Include Encrollments
            var model = await _mediator.Send(query);

            return View(model);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(EditStudentQuery query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EditStudentCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(DeleteStudentQuery query)
        {
            var deleteStudentCommand = await _mediator.Send(query);
            return View(deleteStudentCommand);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteStudentCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

    }
}
