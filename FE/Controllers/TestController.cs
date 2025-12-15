using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class TestController : Controller
    {
        private readonly TestService _service;
        public TestController(TestService service)
        {
            _service = service;
        }

        public IActionResult SelectSubject()
        {
            var subjects = new[] { "Toán", "Vật lý", "Hóa học", "Sinh học", "Ngữ văn" };
            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> Start(string subject)
        {
            try
            {
                var questions = await _service.GetQuestions(subject);
                ViewBag.Subject = subject;
                return View(questions);
            }
            catch
            {
                ViewBag.Error = "Không tải được câu hỏi.";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string subject, Dictionary<int, string> answers)
        {
            var payload = new TestSubmitViewModel
            {
                Subject = subject,
                Answers = answers
            };

            try
            {
                var result = await _service.SubmitTest(payload);
                ViewBag.Subject = subject;
                return View("Result", result);
            }
            catch
            {
                ViewBag.Error = "Không gửi được kết quả kiểm tra.";
                return View("Error");
            }
        }
    }
}
