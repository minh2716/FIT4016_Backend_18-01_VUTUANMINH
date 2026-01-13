using System.ComponentModel.DataAnnotations;

public class School
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Principal { get; set; }

    [Required]
    public string Address { get; set; }

    public ICollection<Student> Students { get; set; }
}
