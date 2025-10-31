using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly RegistrationService _registrationService;

        public RegistrationsController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        public IActionResult Register(string grade, string level)
        {
            var model = new RegistrationViewModel
            {
                Grade = grade,
                Level = level
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
                return View(model);
            }

            var success = await _registrationService.RegisterStudentAsync(model);
            if (success)
            {
                TempData["Success"] = "Đăng ký thành công! Chúng tôi sẽ liên hệ bạn sớm nhất.";
                return RedirectToAction("Register", new { grade = model.Grade, level = model.Level });
            }
            else
            {
                TempData["Error"] = "Đăng ký thất bại. Vui lòng thử lại.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
    }
}
