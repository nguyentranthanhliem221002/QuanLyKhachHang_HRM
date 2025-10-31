using Microsoft.AspNetCore.Identity;
using System;

namespace BE.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int Gender { get; set; } = 0;
        public DateTime? DateOfBirth { get; set; }
        public string RoleType { get; set; } = "Student";

        // Quan hệ 1-1
        public virtual Student? StudentProfile { get; set; }
        public virtual Employee? EmployeeProfile { get; set; }
    }
}
