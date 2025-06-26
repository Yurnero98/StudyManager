using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ModernDesign.DataBase;

public class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    [ForeignKey("Course")]
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}
