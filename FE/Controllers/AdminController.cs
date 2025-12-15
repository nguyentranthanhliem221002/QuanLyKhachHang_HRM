using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly AdminService _adminService;

        private readonly PaymentService _paymentService;

        public AdminController(UserService userService, AdminService adminService, PaymentService paymentService)
        {
            _userService = userService;
            _adminService = adminService;
            _paymentService = paymentService;
        }


        public async Task<IActionResult> Dashboard()
        {
            var (students, teachers, courses) = await _adminService.GetDashboardStatsAsync();

            ViewBag.TotalStudents = students;
            ViewBag.TotalTeachers = teachers;
            ViewBag.TotalCourses = courses;

            return View();
        }

        public async Task<IActionResult> ManageStudents()
        {
            var students = await _userService.GetAllStudentsAsync();
            return View(students); 
        }

        public async Task<IActionResult> ManageEmployees()
        {
            var employees = await _userService.GetAllEmployeesAsync();
            return View(employees); 
        }

        public IActionResult ManageCourses() => View();
        public async Task<IActionResult> ManagePayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments); 
        }

    }
}
