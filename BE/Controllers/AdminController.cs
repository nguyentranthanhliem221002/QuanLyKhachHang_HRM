using BE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalStudents = await _context.Students.CountAsync(s => s.Status == 0);
            var totalEmployees = await _context.Employees.CountAsync(e => e.Status == 0);
            var totalCourses = await _context.Courses.CountAsync(); // Nếu không có: đổi thành 0

            var stats = new
            {
                Students = totalStudents,
                Employees = totalEmployees,
                Courses = totalCourses
            };

            return Ok(stats);
        }
    }
}

