using System.ComponentModel.DataAnnotations;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; }

    [Required, StringLength(20, MinimumLength = 5)]
    public string StudentId { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone must be 10-11 digits")]
    public string? Phone { get; set; }

    [Required]
    public int SchoolId { get; set; }

    public School School { get; set; }
}
