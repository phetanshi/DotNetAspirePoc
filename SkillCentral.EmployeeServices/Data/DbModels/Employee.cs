using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillCentral.EmployeeServices.Data.DbModels;

[Table(name: "tblEmployees")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
    public int? TotalExpInYears { get; set; } = 0;
    public int? TotalExpInMonths { get; set; } = 0;


    public bool IsActive { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string? CreatedUserId { get; set; }
    public string? UpdatedUserId { get; set; }
}
