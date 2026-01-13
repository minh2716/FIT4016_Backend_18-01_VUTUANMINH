using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement
{
    public class Student
    {
        public int Id { get; set; } // primary key

        [Required]
        public int SchoolId { get; set; } // foreign key

        public School? School { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string StudentId { get; set; } = string.Empty; // unique

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // unique

        [Phone]
        [RegularExpression(@"^\d{10,11}$")]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
