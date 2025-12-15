using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly RegistrationService _registrationService;
        private readonly CourseService _courseService;

        public RegistrationsController(RegistrationService registrationService, CourseService courseService)
        {
            _registrationService = registrationService;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            ViewBag.Grades = courses.Select(c => c.Grade).Distinct().OrderBy(g => g).ToList();
            ViewBag.Levels = courses.Select(c => c.Level).Distinct().OrderBy(l => l).ToList();
            ViewBag.Courses = courses;

            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var model = new RegistrationViewModel
            {
                UserId = string.IsNullOrEmpty(userIdClaim) ? Guid.Empty : Guid.Parse(userIdClaim)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid || model.UserId == Guid.Empty || model.CourseId == Guid.Empty)
            {
                TempData["Error"] = "Vui lòng chọn đầy đủ thông tin và đăng nhập trước.";
                return RedirectToAction(nameof(Register));
            }

            model.Status = 0; // Pending

            var registered = await _registrationService.RegisterStudentAsync(model);
            if (!registered)
            {
                TempData["Error"] = "Đăng ký thất bại. Vui lòng thử lại.";
                return RedirectToAction(nameof(Register));
            }

            var course = await _courseService.GetCourseByIdAsync(model.CourseId);
            if (course == null)
            {
                TempData["Error"] = "Khóa học không tồn tại.";
                return RedirectToAction(nameof(Register));
            }

            var payUrl = await _registrationService.PayWithMoMoAsync(model.UserId, model.CourseId, course.Fee);
            return Redirect(payUrl);
        }

        [HttpGet]
        public IActionResult RegisterSuccess() => View();
    }
}
