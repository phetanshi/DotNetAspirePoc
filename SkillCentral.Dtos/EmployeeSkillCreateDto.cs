namespace SkillCentral.Dtos;

public class EmployeeSkillCreateDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SkillId { get; set; }
    public int? YearsOfExpInTheSkill { get; set; }
    public int? MonthsOfExpInTheSkill { get; set; }
}
