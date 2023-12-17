using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data;

namespace SkillCentral.NotificationServices.Services
{
    public class InAppNotificationService(NotificationDbContext database, IMapper mapper, ILogger<InAppNotificationService> logger) : INotificationService
    {
        public Task<NotificationDto> CompletedAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDto> CreateAsync(NotificationCreateDto notificationDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationDto>> GetAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}