using BE.Data;
using BE.Dtos.Requests;
using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;  // ✅ Thêm DbContext

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest req)
        {
            if (await _userManager.FindByEmailAsync(req.Email) != null)
                return BadRequest(new { message = "Email đã tồn tại!" });

            var user = new User
            {
                UserName = req.UserName,
                Email = req.Email,
                FullName = req.FullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, req.Password);
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            // ✅ Gán role mặc định là "Student"
            await _userManager.AddToRoleAsync(user, "Student");

            // ✅ Tạo hồ sơ Student tương ứng
            var student = new Student
            {
                UserId = user.Id,
                StudentCode = "SV" + DateTime.Now.Ticks.ToString().Substring(10),
                EnrollmentDate = DateTime.Now,
                Status = 0
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo tài khoản học viên thành công",
                user = new
                {
                    user.FullName,
                    user.Email,
                    user.UserName,
                    Role = "Student",
                    student.StudentCode
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.UserName);
            if (user == null)
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                username = user.UserName,
                fullname = user.FullName,
                role = roles.FirstOrDefault() ?? "Student",
                message = "Đăng nhập thành công"
            });
        }
    }
}
