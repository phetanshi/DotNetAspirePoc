namespace SkillCentral.Dtos;

public class NotificationCreateDto
{
    public string Notification { get; set; }
    public bool IsCompleted { get; set; } = false;
    public string UserId { get; set; }
}
