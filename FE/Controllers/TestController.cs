using FE.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public TestController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IActionResult SelectSubject()
        {
            // Hiển thị danh sách 5 môn có thể test
            var subjects = new[] { "Toán", "Vật lý", "Hóa học", "Sinh học", "Ngữ văn" };
            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> Start(string subject)
        {
            var client = _factory.CreateClient("BE");
            var response = await client.GetAsync($"api/Test/GetQuestions/{subject}");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Không tải được câu hỏi.";
                return View("Error");
            }

            // Đây nên là danh sách QuestionViewModel
            var questions = await response.Content.ReadFromJsonAsync<List<QuestionViewModel>>();
            ViewBag.Subject = subject;
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string subject, Dictionary<int, string> answers)
        {
            var client = _factory.CreateClient("BE");

            // Tạo payload kiểu TestSubmitModel
            var payload = new TestSubmitModel
            {
                Subject = subject,
                Answers = answers
            };

            var response = await client.PostAsJsonAsync("api/Test/Submit", payload);

            // Nhận kết quả trả về từ BE
            var result = await response.Content.ReadFromJsonAsync<TestResultViewModel>();

            ViewBag.Subject = subject;
            return View("Result", result);
        }

    }

}
