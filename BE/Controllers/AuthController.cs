using BE.Data;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context) => _context = context;

        public class LoginRequest { public string UserName { get; set; } = null!; public string Password { get; set; } = null!; }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == req.UserName && u.Password == req.Password);
            if (user == null) return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

            return Ok(new
            {
                username = user.UserName,
                role = user.Role,
                message = "Đăng nhập thành công"
            });
        }
    }

}
