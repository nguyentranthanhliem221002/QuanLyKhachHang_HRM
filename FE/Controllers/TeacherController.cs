using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Attendance()
        {
            return View();
        }
        public IActionResult MyClasses()
        {
            return View();
        }
        public IActionResult Feedback()
        {
            return View();
        }
    }
}
