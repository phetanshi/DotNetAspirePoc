namespace SkillCentral.Dtos;

public class EmployeeCreateDto
{
    public string? UserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
    public int? TotalExpInYears { get; set; }
    public int? TotalExpInMonths { get; set; }
}
