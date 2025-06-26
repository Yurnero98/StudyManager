using System.ComponentModel.DataAnnotations;

namespace ModernDesign.DataBase;

public class Course
{
    [Key]
    public int CourseId { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(255)]
    public string? Description { get; set; }      
}
