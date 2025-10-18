using BE.Data;
using FE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetQuestions/{subjectName}")]
        public async Task<IActionResult> GetQuestions(string subjectName)
        {
            var subject = await _context.Subjects
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Name.ToLower() == subjectName.ToLower());

            if (subject == null)
                return NotFound(new { message = "Không tìm thấy môn học!" });

            var questions = subject.Questions.Take(20).Select(q => new
            {
                q.Id,
                q.Content,
                q.OptionA,
                q.OptionB,
                q.OptionC,
                q.OptionD
            });

            return Ok(questions);
        }
        [HttpPost("Submit")]
        public IActionResult Submit([FromBody] TestSubmitModel model)
        {
            var answers = model.Answers;
            var subject = model.Subject;

            // Lấy câu hỏi đúng từ DB
            var correctAnswers = _context.Questions
                .Where(q => answers.Keys.Contains(q.Id))
                .ToDictionary(q => q.Id, q => q.CorrectAnswer);

            int score = answers.Count(a => correctAnswers[a.Key] == a.Value) * 5;

            string level = score switch
            {
                <= 50 => "Trung bình",
                <= 80 => "Khá",
                _ => "Giỏi"
            };

            return Ok(new { subject, score, level });
        }

    }
}
