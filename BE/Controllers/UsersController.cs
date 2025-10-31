//using BE.Dtos.Requests;
//using BE.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace BE.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UsersController : ControllerBase
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

//        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        // ===========================
//        // 🔹 Lấy toàn bộ user
//        // ===========================
//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var users = _userManager.Users.Select(u => new
//            {
//                u.Id,
//                u.UserName,
//                u.Email,
//                u.FullName,
//                u.IsActive
//            }).ToList();

//            return Ok(users);
//        }

//        // ===========================
//        // 🔹 Lấy user theo ID
//        // ===========================
//        [HttpGet("{id:guid}")]
//        public async Task<IActionResult> GetById(Guid id)
//        {
//            var user = await _userManager.FindByIdAsync(id.ToString());
//            if (user == null) return NotFound(new { message = "Không tìm thấy người dùng" });

//            var roles = await _userManager.GetRolesAsync(user);

//            return Ok(new
//            {
//                user.Id,
//                user.UserName,
//                user.Email,
//                user.FullName,
//                user.IsActive,
//                Roles = roles
//            });
//        }

//        // ===========================
//        // 🔹 Tạo user mới (Admin)
//        // ===========================
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
//        {
//            if (await _userManager.FindByNameAsync(req.UserName) != null)
//                return BadRequest(new { message = "Tên đăng nhập đã tồn tại" });

//            var user = new User
//            {
//                UserName = req.UserName,
//                Email = req.Email,
//                FullName = req.FullName,
//                IsActive = true,
//                EmailConfirmed = true
//            };

//            var result = await _userManager.CreateAsync(user, req.Password);
//            if (!result.Succeeded)
//                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

//            // Tạo role nếu chưa có
//            if (!await _roleManager.RoleExistsAsync(req.Role))
//                await _roleManager.CreateAsync(new IdentityRole<Guid>(req.Role));

//            // Gán role cho user
//            await _userManager.AddToRoleAsync(user, req.Role);

//            return Ok(new { message = $"Tạo user {req.UserName} thành công với vai trò {req.Role}" });
//        }

//        // ===========================
//        // 🔹 Xóa user
//        // ===========================
//        [HttpDelete("{id:guid}")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var user = await _userManager.FindByIdAsync(id.ToString());
//            if (user == null) return NotFound(new { message = "Không tìm thấy người dùng" });

//            var result = await _userManager.DeleteAsync(user);
//            if (!result.Succeeded)
//                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

//            return Ok(new { message = $"Đã xóa người dùng {user.UserName} thành công" });
//        }

//        // ===========================
//        // 🔹 Lấy danh sách nhân viên
//        // ===========================
//        [HttpGet("employees")]
//        public async Task<IActionResult> GetEmployees()
//        {
//            var users = _userManager.Users.ToList();
//            var employeeList = new List<object>();

//            foreach (var user in users)
//            {
//                var roles = await _userManager.GetRolesAsync(user);
//                if (roles.Contains("Teacher"))
//                {
//                    employeeList.Add(new
//                    {
//                        user.Id,
//                        user.FullName,
//                        user.Email,
//                        user.UserName,
//                        Role = "Teacher"
//                    });
//                }
//            }

//            return Ok(employeeList);
//        }

//        // ===========================
//        // 🔹 Lấy danh sách học viên
//        // ===========================
//        [HttpGet("students")]
//        public async Task<IActionResult> GetStudents()
//        {
//            var users = _userManager.Users.ToList();
//            var studentList = new List<object>();

//            foreach (var user in users)
//            {
//                var roles = await _userManager.GetRolesAsync(user);
//                if (roles.Contains("Student"))
//                {
//                    studentList.Add(new
//                    {
//                        user.Id,
//                        user.FullName,
//                        user.Email,
//                        user.UserName,
//                        Role = "Student"
//                    });
//                }
//            }

//            return Ok(studentList);
//        }
//    }
//}
using BE.Data;
using BE.Dtos.Requests;
using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        // 🔹 Lấy tất cả user (học viên + nhân viên)
        // ===========================
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.StudentProfile)
                .Include(u => u.EmployeeProfile)
                .ToListAsync();

            var list = users.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.UserName,
                Role = u.RoleType, // "Student" hoặc "Teacher"
                ClassName = u.StudentProfile != null ? u.StudentProfile.ClassName : null,
                EnrollmentDate = u.StudentProfile != null ? (DateTime?)u.StudentProfile.EnrollmentDate : null,
                Phone = u.EmployeeProfile != null ? u.EmployeeProfile.Phone : null,
                Position = u.EmployeeProfile != null ? u.EmployeeProfile.Position : null,
                Level = u.EmployeeProfile != null ? u.EmployeeProfile.Level : null,
                Salary = u.EmployeeProfile != null ? (decimal?)u.EmployeeProfile.Salary : null,
                Status = u.EmployeeProfile != null ? (int?)u.EmployeeProfile.Status : (u.StudentProfile != null ? 1 : (int?)null)
            });


            return Ok(list);
        }
        // ===========================
        // 🔹 Tạo học viên
        // ===========================// ===========================
        [HttpPost("students")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest req)
        {
            if (await _userManager.FindByEmailAsync(req.Email) != null)
                return BadRequest(new { message = "Email đã tồn tại" });

            var user = new User
            {
                UserName = req.Email,
                Email = req.Email,
                FullName = req.FullName,
                IsActive = true,
                RoleType = "Student",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "Student@1234");
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            var student = new Student
            {
                UserId = user.Id,
                ClassName = req.ClassName,
                EnrollmentDate = req.EnrollmentDate,
                Status = req.Status
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            if (!await _roleManager.RoleExistsAsync("Student"))
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Student"));

            await _userManager.AddToRoleAsync(user, "Student");

            return Ok(new { message = $"Tạo học viên {req.FullName} thành công" });
        }


        // ===========================
        // 🔹 Sửa học viên
        // ===========================
        //[HttpPut("students/{id:guid}")]
        //public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] CreateStudentRequest req)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null) return NotFound(new { message = "Không tìm thấy học viên" });

        //    user.FullName = req.FullName;
        //    user.Email = req.Email;

        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //        return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

        //    var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == id);
        //    if (student != null)
        //    {
        //        student.ClassName = req.ClassName;
        //        student.EnrollmentDate = req.EnrollmentDate;
        //        await _context.SaveChangesAsync();
        //    }

        //    return Ok(new { message = $"Cập nhật học viên {req.FullName} thành công" });
        //}
        [HttpPut("students/{id:guid}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest req)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { message = "Không tìm thấy học viên" });

            // ✅ Cập nhật thông tin user
            user.FullName = req.FullName;
            user.Email = req.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            // ✅ Cập nhật StudentProfile
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == id);
            if (student != null)
            {
                student.ClassName = req.ClassName;
                student.EnrollmentDate = req.EnrollmentDate;
                student.Status = req.Status;
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = $"Cập nhật học viên {req.FullName} thành công" });
        }

        // ===========================
        // 🔹 Xóa học viên
        // ===========================
        [HttpDelete("students/{id:guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new { message = "Không tìm thấy học viên" });

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == id);
            if (student != null)
                _context.Students.Remove(student);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã xóa học viên {user.FullName} thành công" });
        }

        // ===========================
        // 🔹 Tạo nhân viên
        // ===========================
        [HttpPost("employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest req)
        {
            if (await _userManager.FindByEmailAsync(req.Email) != null)
                return BadRequest(new { message = "Email đã tồn tại" });

            var user = new User
            {
                UserName = req.Email,
                Email = req.Email,
                FullName = req.FullName,
                IsActive = req.Status == 0,
                EmailConfirmed = true,
                RoleType = "Teacher",
            };

            // Password mặc định cứng
            var result = await _userManager.CreateAsync(user, "Teacher@1234");
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            var employee = new Employee
            {
                UserId = user.Id,
                Phone = req.Phone,
                Position = req.Position,
                Level = req.Level,
                Salary = req.Salary,
                Status = req.Status
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            if (!await _roleManager.RoleExistsAsync("Teacher"))
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Teacher"));

            await _userManager.AddToRoleAsync(user, "Teacher");

            return Ok(new { message = $"Tạo nhân viên {req.FullName} thành công" });
        }

        // 🔹 Lấy nhân viên theo ID
        [HttpGet("employees/{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.EmployeeProfile)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null || user.EmployeeProfile == null)
                return NotFound(new { message = "Không tìm thấy nhân viên" });

            var employee = new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.UserName,
                user.EmployeeProfile.Phone,
                user.EmployeeProfile.Position,
                user.EmployeeProfile.Level,
                user.EmployeeProfile.Salary,
                user.EmployeeProfile.Status
            };

            return Ok(employee);
        }

        // ===========================
        // 🔹 Sửa nhân viên
        // ===========================
        //[HttpPut("employees/{id:guid}")]
        //public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] CreateEmployeeRequest req)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null) return NotFound(new { message = "Không tìm thấy nhân viên" });

        //    user.FullName = req.FullName;
        //    user.Email = req.Email;
        //    user.IsActive = req.Status == 1;

        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //        return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

        //    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == id);
        //    if (employee != null)
        //    {
        //        employee.Phone = req.Phone;
        //        employee.Position = req.Position;
        //        employee.Level = req.Level;
        //        employee.Salary = req.Salary;
        //        employee.Status = req.Status;
        //        await _context.SaveChangesAsync();
        //    }

        //    return Ok(new { message = $"Cập nhật nhân viên {req.FullName} thành công" });
        //}
        [HttpPut("employees/{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeRequest req)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { message = "Không tìm thấy nhân viên" });

            // ✅ Cập nhật thông tin user
            user.FullName = req.FullName;
            user.Email = req.Email;
            user.IsActive = req.Status == 1;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            // ✅ Cập nhật EmployeeProfile
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == id);
            if (employee != null)
            {
                employee.Phone = req.Phone;
                employee.Position = req.Position;
                employee.Level = req.Level;
                employee.Salary = req.Salary;
                employee.Status = req.Status;
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = $"Cập nhật nhân viên {req.FullName} thành công" });
        }

        // ===========================
        // 🔹 Xóa nhân viên
        // ===========================
        [HttpDelete("employees/{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new { message = "Không tìm thấy nhân viên" });

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == id);
            if (employee != null)
                _context.Employees.Remove(employee);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã xóa nhân viên {user.FullName} thành công" });
        }

        // ===========================
        // 🔹 Lấy danh sách học viên
        // ===========================
        //[HttpGet("students")]
        //public async Task<IActionResult> GetStudents()
        //{
        //    var students = await _context.Users
        //        .Include(u => u.StudentProfile)
        //        .Where(u => u.StudentProfile != null)
        //        .ToListAsync();

        //    var list = students.Select(u => new
        //    {
        //        u.Id,
        //        u.FullName,
        //        u.Email,
        //        u.UserName,
        //        u.DateOfBirth,
        //        u.StudentProfile.ClassName,
        //        u.StudentProfile.EnrollmentDate
        //    });

        //    return Ok(list);
        //}
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents(
        [FromQuery] string? search,
        [FromQuery] string? grade,
        [FromQuery] string? level,
        [FromQuery] string? status)
        {
            var query = _context.Users
                .Include(u => u.StudentProfile)
                .Where(u => u.StudentProfile != null)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u =>
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search));

            if (!string.IsNullOrWhiteSpace(grade))
                query = query.Where(u => u.StudentProfile.Grade == grade);

            if (!string.IsNullOrWhiteSpace(level))
                query = query.Where(u => u.StudentProfile.Level == level);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(u =>
                    u.StudentProfile.Status.ToString() == status ||
                    (status == "Pending" && u.StudentProfile.Status == 0) ||
                    (status == "Approved" && u.StudentProfile.Status == 1));

            var list = await query.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.StudentProfile.Grade,
                u.StudentProfile.Level,
                u.StudentProfile.Status
            }).ToListAsync();

            return Ok(list);
        }

        // ===========================
        // 🔹 Lấy danh sách nhân viên
        // ===========================
        //[HttpGet("employees")]
        //public async Task<IActionResult> GetEmployees()
        //{
        //    var employees = await _context.Users
        //        .Include(u => u.EmployeeProfile)
        //        .Where(u => u.EmployeeProfile != null)
        //        .ToListAsync();

        //    var list = employees.Select(u => new
        //    {
        //        u.Id,
        //        u.FullName,
        //        u.Email,
        //        u.UserName,
        //        u.EmployeeProfile.Phone,
        //        u.EmployeeProfile.Position,
        //        u.EmployeeProfile.Level,
        //        u.EmployeeProfile.Salary,
        //        u.EmployeeProfile.Status
        //    });

        //    return Ok(list);
        //}
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees(
        [FromQuery] string? search,
        [FromQuery] string? position,
        [FromQuery] string? level,
        [FromQuery] string? status)
        {
            var query = _context.Users
                .Include(u => u.EmployeeProfile)
                .Where(u => u.EmployeeProfile != null)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u =>
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.EmployeeProfile.Phone.Contains(search));

            if (!string.IsNullOrWhiteSpace(position))
                query = query.Where(u => u.EmployeeProfile.Position == position);

            if (!string.IsNullOrWhiteSpace(level))
                query = query.Where(u => u.EmployeeProfile.Level == level);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(u =>
                    u.EmployeeProfile.Status.ToString() == status ||
                    (status == "Active" && u.EmployeeProfile.Status == 1) ||
                    (status == "Inactive" && u.EmployeeProfile.Status == 0));

            var list = await query.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.UserName,
                u.EmployeeProfile.Phone,
                u.EmployeeProfile.Position,
                u.EmployeeProfile.Level,
                u.EmployeeProfile.Salary,
                u.EmployeeProfile.Status
            }).ToListAsync();

            return Ok(list);
        }

    }
}
