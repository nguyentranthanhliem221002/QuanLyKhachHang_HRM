using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        public IActionResult Dashboard() => View();

        public IActionResult ManageStudents() => View();

        public IActionResult ManageCourses() => View();

        public IActionResult ManageEmployees() => View();

    }
}
