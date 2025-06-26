using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ModernDesign.DataBase;

public class Student
{
    [Key]
    public int StudentId { get; set; }
    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    [ForeignKey("Group")]
    public int? GroupId { get; set; }
    public Group? Group { get; set; }
}
