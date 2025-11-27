using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class EmployeeController : Controller
    {
        private readonly UserService _userService;

        public EmployeeController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Dashboard()
        {
            var employees = await _userService.GetAllEmployeesAsync();

            // Lấy nhân viên hiện tại dựa trên email đăng nhập
            var currentEmployee = employees.FirstOrDefault(e => e.UserName == User.Identity?.Name);

            if (currentEmployee == null)
                return NotFound("Không tìm thấy thông tin nhân viên.");

            return View(currentEmployee);
        }

        public IActionResult Attendance() => View();
        public IActionResult MyClasses() => View();
        public IActionResult Feedback() => View();
    }
}
