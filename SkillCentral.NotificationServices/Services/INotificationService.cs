using SkillCentral.Dtos;

namespace SkillCentral.NotificationServices.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetAsync(string userId);
        Task<List<NotificationDto>> GetAdminAsync();
        Task<List<NotificationDto>> GetSupportAsync();
        Task<NotificationDto> CreateAsync(NotificationCreateDto notificationDto);
        Task<NotificationDto> CompletedAsync(int notificationId);
        Task<bool> DeleteAsync(int notificationId);
    }
}
