using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillCentral.SkillServices.Data.DbModels;

[Table(name: "tblSkills")]
public class Skill
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }

    public bool IsActive { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string? CreatedUserId { get; set; }
    public string? UpdatedUserId { get; set; }
}
