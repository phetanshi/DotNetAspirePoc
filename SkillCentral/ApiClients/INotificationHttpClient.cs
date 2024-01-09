using SkillCentral.Dtos;

namespace SkillCentral.ApiClients;

public interface INotificationHttpClient
{
    Task<List<NotificationDto>> GetAdminNotificationsAsync();
    Task<List<NotificationDto>> GetEmployeeNotificationsAsync(string userId);
    Task MarkAsComplete(int notificationId);
}
