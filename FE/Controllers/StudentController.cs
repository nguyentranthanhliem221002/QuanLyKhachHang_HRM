using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FE.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UserService _userService;
        private readonly CourseService _courseService;
        private readonly PaymentService _paymentService;


        public StudentController(UserService userService, CourseService courseService, PaymentService paymentService)
        {
            _userService = userService;
            _paymentService = paymentService;

            _courseService = courseService;
        }

        public IActionResult Dashboard() => View();
        public IActionResult Profile() => View();

        public async Task<IActionResult> MyCourse()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            var courses = await _courseService.GetCoursesByStudentIdAsync(userId);

            var model = new StudentViewModel
            {
                MyCourses = courses
            };
            return View(model);
        }
        public IActionResult Feedback() => View();
    }
}
