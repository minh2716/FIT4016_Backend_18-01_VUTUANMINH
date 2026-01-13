using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement
{
    public class School
    {
        public int Id { get; set; } // primary key

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; // unique

        [Required]
        [MaxLength(100)]
        public string Principal { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<Student> Students { get; set; } = new();
    }
}
