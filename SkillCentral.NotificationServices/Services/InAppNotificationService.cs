using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data.DbModels;
using SkillCentral.Repository;

namespace SkillCentral.NotificationServices.Services
{
    public class InAppNotificationService(IRepository repository, IMapper mapper, ILogger<InAppNotificationService> logger, IHttpContextAccessor context) : ServiceBase(context), INotificationService
    {
        public async Task<NotificationDto> CompletedAsync(int notificationId)
        {
            var notification = await repository.GetSingleAsync<Notification>(notificationId);
            if (notification == null) return null;
            
            notification.IsCompleted = true;
            notification.DateUpdated = DateTime.UtcNow;
            notification.UpdatedUserId = GetLoginUserId();
            await repository.UpdateAsync(notification);

            return mapper.Map<NotificationDto>(notification);
        }

        public async Task<NotificationDto> CreateAsync(NotificationCreateDto notificationDto)
        {
            var dbObj = mapper.Map<Notification>(notificationDto);
            dbObj.CreatedUserId = GetLoginUserId();
            dbObj.DateCreated = DateTime.UtcNow;

            dbObj = await repository.CreateAsync(dbObj);

            if (dbObj is null)
                return null;

            return mapper.Map<NotificationDto>(dbObj);
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification = await repository.GetSingleAsync<Notification>(notificationId);
            if (notification == null) return false;

            notification.IsActive = false;
            notification.UpdatedUserId = GetLoginUserId();
            notification.DateUpdated = DateTime.UtcNow;

            int count = await repository.UpdateAsync(notification);

            return count > 0;
        }

        public async Task<List<NotificationDto>> GetAsync(string userId)
        {
            var dbData = await repository.GetListAsync<Notification>(x => x.UserId.ToLower() == userId.ToLower());
            return mapper.Map<List<NotificationDto>>(dbData);
        }
    }
}