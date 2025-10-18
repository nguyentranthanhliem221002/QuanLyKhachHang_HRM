using BE.Model;
using BE.Models;

namespace BE.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // ✅ Seed Users
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { UserName = "admin", Password = "admin123", Role = "Admin", FullName="Admin System", Email="admin@local" },
                    new User { UserName = "teacher1", Password = "teach123", Role = "Teacher", FullName="Thầy A", Email="teacher@local" },
                    new User { UserName = "student1", Password = "stud123", Role = "Student", FullName="Sinh Viên A", Email="student@local" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // ✅ Seed Subjects
            if (!context.Subjects.Any())
            {
                var subjects = new List<Subject>
                {
                    new Subject { Name = "Toán" },
                    new Subject { Name = "Vật lý" },
                    new Subject { Name = "Hóa học" },
                    new Subject { Name = "Sinh học" },
                    new Subject { Name = "Ngữ văn" },
                    new Subject { Name = "Kỹ năng mềm" } // Không tạo câu hỏi
                };
                context.Subjects.AddRange(subjects);
                context.SaveChanges();
            }

            // ✅ Seed Questions (mỗi môn 20 câu, trừ kỹ năng mềm)
            if (!context.Questions.Any())
            {
                int questionId = 1;
                for (int subjectId = 1; subjectId <= 5; subjectId++)
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        string subjectName = subjectId switch
                        {
                            1 => "Toán",
                            2 => "Vật lý",
                            3 => "Hóa học",
                            4 => "Sinh học",
                            5 => "Ngữ văn",
                            _ => ""
                        };

                        string content = subjectName switch
                        {
                            "Toán" => $"{i}. 1 + {i} = ?",
                            "Vật lý" => $"{i}. Vận tốc vật thể thứ {i} là bao nhiêu?",
                            "Hóa học" => $"{i}. Nguyên tố hóa học số {i} là gì?",
                            "Sinh học" => $"{i}. Câu hỏi sinh học số {i} liên quan đến tế bào",
                            "Ngữ văn" => $"{i}. Trong văn bản số {i}, tác giả nhấn mạnh điều gì?",
                            _ => ""
                        };
                        context.Questions.Add(new Question
                        {
                            SubjectId = subjectId,
                            Content = content,
                            OptionA = "A",
                            OptionB = "B",
                            OptionC = "C",
                            OptionD = "D",
                            CorrectAnswer = "B"
                        });

                    }
                }
                context.SaveChanges();
            }
        }
    }
}
