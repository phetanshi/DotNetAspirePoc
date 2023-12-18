namespace SkillCentral.Dtos;

public class NotificationDto
{
    public NotificationDto()
    {
        Employee = new EmployeeDto();
    }
    public int Id { get; set; }
    public string Notification { get; set; }
    public bool IsCompleted { get; set; }
    public EmployeeDto? Employee { get; set; }
}
