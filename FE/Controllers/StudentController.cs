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
        //public async Task<IActionResult> MyCourse()
        //{
        //    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

        //    // Lấy danh sách payments của user
        //    var payments = await _paymentService.GetHistoryAsync(userId);

        //    var model = new StudentViewModel
        //    {
        //        MyCourses = new List<CourseViewModel>()
        //    };

        //    foreach (var payment in payments)
        //    {
        //        // Lấy thông tin course
        //        var course = await _courseService.GetCourseByIdAsync(payment.CourseId);
        //        if (course != null)
        //        {
        //            model.MyCourses.Add(new CourseViewModel
        //            {
        //                Id = course.Id,
        //                Title = course.Title,
        //                SubjectName = course.SubjectName,
        //                StartDate = course.StartDate,
        //                EndDate = course.EndDate,
        //                Fee = payment.Amount,
        //                IsPaid = payment.IsPaid,
        //                TransactionId = payment.OrderId // ← đây là quan trọng
        //            });
        //        }
        //    }

        //    return View(model);
        //}

        public IActionResult Feedback() => View();
    }
}
