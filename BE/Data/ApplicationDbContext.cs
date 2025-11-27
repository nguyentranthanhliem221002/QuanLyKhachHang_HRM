//using BE.Model;
//using BE.Models;
//using Microsoft.EntityFrameworkCore;

//namespace BE.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options) { }
//        public DbSet<User> Users { get; set; }
//        public DbSet<Person> Persons { get; set; }
//        public DbSet<Course> Courses { get; set; }
//        public DbSet<Employee> Employees { get; set; }
//        public DbSet<Student> Students { get; set; }
//        public DbSet<Payment> Payments { get; set; }
//        public DbSet<Feedback> Feedbacks { get; set; }
//        public DbSet<Schedule> Schedules { get; set; }
//        public DbSet<Notification> Notifications { get; set; }
//        public DbSet<Enrollment> Enrollments { get; set; }
//        public DbSet<Department> Departments { get; set; }
//        public DbSet<Subject> Subjects { get; set; }
//        public DbSet<Question> Questions { get; set; }

//    }
//}

using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BE.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Các DbSet khác của bạn
        public DbSet<Course> Courses { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
