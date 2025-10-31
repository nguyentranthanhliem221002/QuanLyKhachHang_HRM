using FE.Services;
using FE.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _service.GetAllCoursesAsync();
            return View(courses);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var course = await _service.GetCourseByIdAsync(id);
            return View(course);
        }
    }
}
