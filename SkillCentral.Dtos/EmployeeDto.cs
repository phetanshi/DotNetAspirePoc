namespace SkillCentral.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
    public int? TotalExpInYears { get; set; } = 0;
    public int? TotalExpInMonths { get; set; } = 0;
}
