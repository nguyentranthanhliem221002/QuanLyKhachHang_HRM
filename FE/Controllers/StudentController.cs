using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult MyCourse()
        {
            return View();
        }
        public IActionResult Feedback()
        {
            return View();
        }
    }
}
