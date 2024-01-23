using System.ComponentModel.DataAnnotations;

namespace SkillCentral.Dtos;

public class SkillCreateDto
{
    [Required]
    [StringLength(250, ErrorMessage = "Skill name length can't be more than 250")]
    [Display(Name = "Skill Name")]
    public string Name { get; set; }
}
