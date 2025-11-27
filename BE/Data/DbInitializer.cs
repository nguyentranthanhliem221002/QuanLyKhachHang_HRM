using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BE.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {


            // =======================
            // 1️⃣ Seed Roles
            // =======================
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }

            // =======================
            // 2️⃣ Seed Users + Profiles
            // =======================
            if (!await userManager.Users.AnyAsync())
            {
                var users = new List<(string username, string email, string fullname, string password, string role)>
                {
                    ("admin", "admin@local", "Admin System", "Admin123!", "Admin"),
                    ("teacher1", "teacher@local", "Thầy A", "Teach123!", "Teacher"),
                    ("student1", "student@local", "Sinh viên A", "Stud123!", "Student")
                };

                foreach (var (username, email, fullname, password, role) in users)
                {
                    var user = new User
                    {
                        UserName = username,
                        Email = email,
                        FullName = fullname,
                        EmailConfirmed = true,
                        IsActive = true,
                        RoleType = role
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Lỗi tạo user '{username}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        Console.ResetColor();
                        continue;
                    }

                    await userManager.AddToRoleAsync(user, role);

                    if (role == "Teacher")
                    {
                        var employee = new Employee
                        {
                            UserId = user.Id,
                            Phone = "0123456789",
                            Position = "Giáo viên Toán",
                            Level = "Senior",
                            Salary = 1500,
                            DateOfJoining = DateTime.Now,
                            Status = 0
                        };
                        context.Employees.Add(employee);
                    }
                    else if (role == "Student")
                    {
                        var student = new Student
                        {
                            UserId = user.Id,
                            StudentCode = "SV001",
                            ClassName = "10A1",
                            EnrollmentDate = DateTime.Now,
                            Status = 0,
                            Grade = "10",
                            Level = "Cơ bản"
                        };
                        context.Students.Add(student);
                    }

                    Console.WriteLine($"✅ Tạo user '{username}' với role '{role}' thành công.");
                }

                await context.SaveChangesAsync();
            }

            // =======================
            // 3️⃣ Seed Subjects
            // =======================
            if (!await context.Subjects.AnyAsync())
            {
                var subjects = new List<Subject>
                {
                    new Subject { Name = "Toán" },
                    new Subject { Name = "Vật lý" },
                    new Subject { Name = "Hóa học" },
                    new Subject { Name = "Sinh học" },
                    new Subject { Name = "Ngữ văn" },
                    new Subject { Name = "Kỹ năng mềm" }
                };

                await context.Subjects.AddRangeAsync(subjects);
                await context.SaveChangesAsync();
                Console.WriteLine("✅ Đã seed dữ liệu môn học.");
            }

            // =======================
            // 4️⃣ Seed Questions
            // =======================
            if (!await context.Questions.AnyAsync())
            {
                var subjects = await context.Subjects
                    .Where(s => s.Name != "Kỹ năng mềm")
                    .ToListAsync();

                foreach (var subject in subjects)
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        await context.Questions.AddAsync(new Question
                        {
                            SubjectId = subject.Id,
                            Content = $"{i}. Câu hỏi {subject.Name} số {i}",
                            OptionA = "A",
                            OptionB = "B",
                            OptionC = "C",
                            OptionD = "D",
                            CorrectAnswer = "B"
                        });
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("✅ Đã seed dữ liệu câu hỏi.");
            }

            if (!await context.Courses.AnyAsync())
            {
                var subjects = await context.Subjects.ToListAsync();
                var courses = new List<Course>();

                var grades = new[] { "10", "11", "12" };
                var levels = new[] { "Trung bình", "Khá", "Giỏi" };

                foreach (var subject in subjects)
                {
                    foreach (var grade in grades)
                    {
                        foreach (var level in levels)
                        {
                            var fee = level == "Trung bình" ? 1000000m :
                                      level == "Khá" ? 1200000m :
                                      1500000m;

                            courses.Add(new Course
                            {
                                Id = Guid.NewGuid(),
                                Title = $"Khóa {subject.Name} - Lớp {grade} - {level}",
                                Description = $"Khóa học {level} lớp {grade} môn {subject.Name}.",
                                Fee = fee,
                                StartDate = DateTime.Now.AddDays(-7),
                                EndDate = DateTime.Now.AddMonths(2),
                                SubjectId = subject.Id,
                                Grade = grade,
                                Level = level
                            });
                        }
                    }
                }

                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
                Console.WriteLine("✅ Đã seed dữ liệu Courses (khóa học) với Id là Guid.");
            }

            Console.WriteLine("✅ Seed dữ liệu ban đầu hoàn tất (không xóa dữ liệu cũ).");
        }
    }
}
