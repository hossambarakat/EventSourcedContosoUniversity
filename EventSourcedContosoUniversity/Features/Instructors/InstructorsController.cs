using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;
using EventSourcedContosoUniversity.Features.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcedContosoUniversity.Features.Instructors
{
    
    public class InstructorsController : Controller
    {
        private readonly IMediator _mediator;

        public InstructorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(GetInstructorsQuery query/*int? id*/)
        {
            var viewModel = await _mediator.Send(query);
            return View(viewModel);
            //TODO: Handle select instructor!
            //if (id != null)
            //{
            //    ViewData["InstructorID"] = id.Value;
            //    Instructor instructor = viewModel.Instructors.Where(
            //        i => i.ID == id.Value).Single();
            //    viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            //}

            
        }


        public async Task<IActionResult> Details(GetInstructorDetailsQuery query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateAssignedCourseData(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstructorCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(EditInstructorQuery query)
        {
            var model = await _mediator.Send(query);
            await PopulateAssignedCourseData(model.SelectedCourses.ToList());
            return View(model);
        }

        private async Task PopulateAssignedCourseData(List<Guid> courseAssignments)
        {
            var allCourses = await _mediator.Send(new GetCoursesQuery());
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.Id,
                    CourseNumber = course.Number,
                    Title = course.Title,
                    Assigned = courseAssignments == null ? false : courseAssignments.Contains(course.Id)
                });
            }
            ViewData["Courses"] = viewModel;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInstructorCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(DeleteInstructorQuery query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteInstructorCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

    }
}
