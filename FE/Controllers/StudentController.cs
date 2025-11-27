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

        public StudentController(UserService userService, CourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        public IActionResult Dashboard() => View();
        public IActionResult Profile() => View();
        //public async Task<IActionResult> MyCourse()
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userIdClaim))
        //    {
        //        TempData["Error"] = "Không thể xác định học viên.";
        //        return RedirectToAction("Dashboard");
        //    }

        //    Guid userId = Guid.Parse(userIdClaim);

        //    // SỬA DÒNG NÀY: TRUYỀN Guid, KHÔNG TRUYỀN ToString()!!!
        //    var myCourses = await _courseService.GetCoursesByStudentIdAsync(userId);

        //    var model = new StudentViewModel
        //    {
        //        MyCourses = myCourses
        //    };

        //    return View(model);
        //}

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
