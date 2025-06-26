using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ModernDesign.DataBase;

public class Group
{
    [Key]
    public int GroupId { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }

    [ForeignKey("Course")]
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
   
    public ICollection<Student>? Students { get; set; }
}
