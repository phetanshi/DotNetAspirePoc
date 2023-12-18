using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data;
using SkillCentral.NotificationServices.Data.DbModels;

namespace SkillCentral.NotificationServices.Services
{
    public class InAppNotificationService(NotificationDbContext database, IMapper mapper, ILogger<InAppNotificationService> logger) : INotificationService
    {
        public async Task<NotificationDto> CompletedAsync(int notificationId)
        {
            var notification = await database.Notifications.FindAsync(notificationId);
            if (notification == null) return null;
            
            notification.IsCompleted = true;
            await database.SaveChangesAsync();
            return mapper.Map<NotificationDto>(notification);
        }

        public async Task<NotificationDto> CreateAsync(NotificationCreateDto notificationDto)
        {
            var dbObj = mapper.Map<Notification>(notificationDto);
            database.Notifications.Add(dbObj);
            int count = await database.SaveChangesAsync();
            if (count == 0) return null;
            return mapper.Map<NotificationDto>(dbObj);
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification = await database.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsActive = false;
            var count = await database.SaveChangesAsync();
            return count > 0;
        }

        public async Task<List<NotificationDto>> GetAsync(string userId)
        {
            var dbData = await database.Notifications.Where(x => string.Equals(x.UserId, userId, StringComparison.OrdinalIgnoreCase)).ToListAsync();
            return mapper.Map<List<NotificationDto>>(dbData);
        }
    }
}