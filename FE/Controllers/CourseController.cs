using FE.Services;
using FE.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            return View(course);
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            ViewBag.Grades = courses.Select(c => c.Grade).Distinct().OrderBy(g => g).ToList();
            ViewBag.Levels = courses.Select(c => c.Level).Distinct().OrderBy(l => l).ToList();
            ViewBag.Courses = courses;

            var model = new RegistrationViewModel();

            // Lấy UserId từ claims
            var userIdClaim = User?.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                model.UserId = Guid.Parse(userIdClaim);
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng chọn đầy đủ thông tin.";
                return RedirectToAction(nameof(Register));
            }

            // Lấy UserId từ Claims
            var userId = User?.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                model.UserId = Guid.Parse(userId);
            }

            // Mặc định Status = 0
            model.Status = 0;

            var success = await _courseService.RegisterStudentAsync(model);

            if (success)
                TempData["Success"] = "Đăng ký thành công!";
            else
                TempData["Error"] = "Môn học hiện này đã được đăng ký trước đó!";

            return RedirectToAction(nameof(Register));
        }

        [HttpGet]
        public IActionResult RegisterSuccess() => View();
    }
}
