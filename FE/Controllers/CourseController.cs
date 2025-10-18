using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Enroll()
        {
            return View();
        }
    }
}
