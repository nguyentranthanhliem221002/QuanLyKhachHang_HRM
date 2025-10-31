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

        //[HttpGet]
        //public async Task<IActionResult> GetAllCourses()
        //{
        //    var courses = await _context.Courses.Include(c => c.Subject).ToListAsync();
        //    return Ok(courses);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetAllCourses()
        //{
        //    var courses = await _context.Courses
        //        .Include(c => c.Subject)
        //        .Select(c => new CourseDTO
        //        {
        //            Id = c.Id,
        //            Title = c.Title,
        //            Description = c.Description,
        //            Fee = c.Fee,
        //            StartDate = c.StartDate,
        //            EndDate = c.EndDate,
        //            SubjectId = c.SubjectId,
        //            SubjectName = c.Subject.Name
        //        })
        //        .ToListAsync();

        //    return Ok(courses);
        //}
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

            // 🔍 Tìm kiếm theo tên hoặc mô tả
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Title.Contains(search) ||
                    c.Description.Contains(search));
            }

            // 🎓 Lọc theo lớp học (Grade)
            if (!string.IsNullOrWhiteSpace(grade))
            {
                query = query.Where(c => c.Grade == grade);
            }

            // 📈 Lọc theo mức học (Level)
            if (!string.IsNullOrWhiteSpace(level))
            {
                query = query.Where(c => c.Level == level);
            }

            // 📘 Lọc theo môn học (Subject)
            if (!string.IsNullOrWhiteSpace(subject))
            {
                query = query.Where(c => c.Subject.Name == subject);
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

        //[HttpPost]
        //public async Task<IActionResult> CreateCourse([FromBody] Course course)
        //{
        //    _context.Courses.Add(course);
        //    await _context.SaveChangesAsync();
        //    return Ok(course);
        //}
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
                SubjectId = model.SubjectId, // ✅ Nhận SubjectId từ FE
                Grade = model.Grade,
                Level = model.Level
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tạo khóa học thành công", Data = course });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Fee = c.Fee,
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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course updatedCourse)
        //{
        //    var course = await _context.Courses.FindAsync(id);
        //    if (course == null) return NotFound();

        //    course.Title = updatedCourse.Title;
        //    course.Description = updatedCourse.Description;
        //    course.Fee = updatedCourse.Fee;
        //    course.StartDate = updatedCourse.StartDate;
        //    course.EndDate = updatedCourse.EndDate;
        //    course.SubjectId = updatedCourse.SubjectId;

        //    await _context.SaveChangesAsync();
        //    return Ok(course);
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest model)
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
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
