namespace SkillCentral.Dtos;

public class EmployeeSkillDto
{
    public EmployeeSkillDto()
    {
        Employee = new EmployeeDto();
        Skill = new SkillDto();
    }
    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public SkillDto? Skill { get; set; }
    public DateTime DateAdded { get; set; }
    public int YearsOfExpInTheSkill { get; set; }
    public int MonthsOfExpInTheSkill { get; set; }
}
