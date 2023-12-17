using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillCentral.SkillServices.Data.DbModels;

[Table(name: "tblEmployeeSkills")]
public class EmployeeSkill
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SkillId { get; set; }
    public int? YearsOfExpInTheSkill { get; set; }
    public int? MonthsOfExpInTheSkill { get; set; }

    public bool IsActive { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string? CreatedUserId { get; set; }
    public string? UpdatedUserId { get; set; }
}
