using BE.Data;
using BE.Dtos;
using BE.Dtos.Requests;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses(
            [FromQuery] string? search,
            [FromQuery] string? grade,
            [FromQuery] string? level,
            [FromQuery] string? subject)
        {
            var query = _context.Courses
                .Include(c => c.Subject)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Title.Contains(search) || c.Description.Contains(search));

            if (!string.IsNullOrWhiteSpace(grade))
                query = query.Where(c => c.Grade == grade);

            if (!string.IsNullOrWhiteSpace(level))
                query = query.Where(c => c.Level == level);

            if (!string.IsNullOrWhiteSpace(subject))
            {
                if (Guid.TryParse(subject, out var subjectId))
                    query = query.Where(c => c.SubjectId == subjectId);
            }

            var courses = await query
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Fee = c.Fee,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Grade = c.Grade,
                    Level = c.Level,
                    SubjectId = c.SubjectId,
                    SubjectName = c.Subject.Name
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                Fee = model.Fee,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                SubjectId = model.SubjectId,
                Grade = model.Grade,
                Level = model.Level
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tạo khóa học thành công", Data = course });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Fee = c.Fee,
                    Level = c.Level,
                    Grade = c.Grade,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    SubjectId = c.SubjectId,
                    SubjectName = c.Subject.Name
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound(new { Message = "Không tìm thấy khóa học." });

            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { Message = "Không tìm thấy khóa học." });

            course.Title = model.Title;
            course.Description = model.Description;
            course.Fee = model.Fee;
            course.StartDate = model.StartDate;
            course.EndDate = model.EndDate;
            course.SubjectId = model.SubjectId;
            course.Grade = model.Grade;
            course.Level = model.Level;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật khóa học thành công", Data = course });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegistrationRequest model)
        {
            if (model.UserId == Guid.Empty)
                return BadRequest(new { Message = "UserId không hợp lệ." });

            if (model.CourseId == Guid.Empty)
                return BadRequest(new { Message = "CourseId không hợp lệ." });

            var userExists = await _context.Users.AnyAsync(u => u.Id == model.UserId);
            if (!userExists)
                return BadRequest(new { Message = "Người dùng không tồn tại." });

            var courseExists = await _context.Courses.AnyAsync(c => c.Id == model.CourseId);
            if (!courseExists)
                return BadRequest(new { Message = "Khóa học không tồn tại." });

            var alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.UserId == model.UserId && r.CourseId == model.CourseId);

            if (alreadyRegistered)
                return BadRequest(new { Message = "Bạn đã đăng ký khóa học này rồi." });

            var registration = new Registration
            {
                UserId = model.UserId,
                CourseId = model.CourseId,
                Grade = model.Grade,
                Level = model.Level,
                Status = model.Status 
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đăng ký khóa học thành công!", Data = registration });
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetCoursesByStudentId(Guid studentId)
        {
            var courses = await _context.Registrations
                .Where(r => r.UserId == studentId)
                .Include(r => r.Course)
                .ThenInclude(c => c.Subject)
                .Select(r => new CourseDTO
                {
                    Id = r.Course.Id,
                    Title = r.Course.Title,
                    Description = r.Course.Description,
                    Fee = r.Course.Fee,
                    StartDate = r.Course.StartDate,
                    EndDate = r.Course.EndDate,
                    SubjectId = r.Course.SubjectId,
                    SubjectName = r.Course.Subject.Name,
                    Grade = r.Course.Grade,
                    Level = r.Course.Level,
                    IsPaid = r.Status == 1 // 1 = Paid, 0 = Pending
                })
                .ToListAsync();

            return Ok(courses);
        }

    }

}
