using BE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả môn học
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _context.Subjects
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            return Ok(subjects);
        }
    }
}
