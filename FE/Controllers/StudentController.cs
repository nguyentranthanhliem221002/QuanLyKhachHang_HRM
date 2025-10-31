using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UserService _userService;

        public StudentController(UserService userService)
        {
            _userService = userService;
        }

        //[HttpGet]
        //public IActionResult Register(string grade, string level)
        //{
        //    var model = new RegistrationViewModel
        //    {
        //        Grade = grade,
        //        Level = level
        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegistrationViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var newUser = new UserViewModel
        //    {
        //        FullName = model.FullName,
        //        Email = model.Email,
        //        RoleType = "Student",
        //        // nếu cần thêm trường khác thì thêm tại đây
        //    };

        //    var created = await _userService.AddUserAsync(newUser);
        //    if (created == null)
        //    {
        //        TempData["Error"] = "Đăng ký thất bại. Vui lòng thử lại.";
        //        return View(model);
        //    }

        //    TempData["Success"] = "Đăng ký thành công!";
        //    return RedirectToAction("Dashboard");
        //}

        public IActionResult Dashboard() => View();
        public IActionResult Profile() => View();
        public IActionResult MyCourse() => View();
        public IActionResult Feedback() => View();
    }
}
