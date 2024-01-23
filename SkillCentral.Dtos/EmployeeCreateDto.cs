using System.ComponentModel.DataAnnotations;

namespace SkillCentral.Dtos;

public class EmployeeCreateDto
{

    [Required]
    [StringLength(25, ErrorMessage = "First name length can't be more than 25")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }


    [Required]
    [StringLength(25, ErrorMessage = "Last name length can't be more than 25")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }


    [StringLength(25, ErrorMessage = "Designation length can't be more than 25")]
    [Display(Name = "Designation")]
    public string? Designation { get; set; }


    [Display(Name = "Experience  (In Years)")]
    public int TotalExpInYears { get; set; } = 0;


    [Display(Name = "Experience  (In Months)")]
    public int TotalExpInMonths { get; set; } = 0;
}
