using AutoMapper;
using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data.DbModels;
using SkillCentral.NotificationServices.Utils;
using SkillCentral.Repository;

namespace SkillCentral.NotificationServices.Services
{
    public class InAppNotificationService(IRepository repository, IMqRequestService requestQueueService, IMapper mapper, ILogger<InAppNotificationService> logger, IHttpContextAccessor context) : ServiceBase(context), INotificationService
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
            Notification dbObj = mapper.Map<Notification>(notificationDto);
            dbObj.CreatedUserId = GetLoginUserId();
            dbObj.DateCreated = DateTime.UtcNow;

            var newDbObj = await repository.CreateAsync(dbObj);

            if (newDbObj is null)
                return null;

            var dto = mapper.Map<NotificationDto>(newDbObj);
            dto.Employee = await BindEmployeeData(newDbObj.UserId);
            return dto;
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

        public async Task<List<NotificationDto>> GetAdminAsync()
        {
            var dbData = await repository.GetListAsync<Notification>(x => x.IsAdmin && x.IsActive && x.IsCompleted == false);
            var dtoList = await dbData.BindDtoList(mapper, BindEmployeeData);
            return dtoList.ToList();
        }
        public async Task<List<NotificationDto>> GetSupportAsync()
        {
            var dbData = await repository.GetListAsync<Notification>(x => x.IsSupport && x.IsActive && x.IsCompleted == false);
            var dtoList = await dbData.BindDtoList(mapper, BindEmployeeData);
            return dtoList.ToList();
        }
        public async Task<List<NotificationDto>> GetAsync(string userId)
        {
            var dbData = await repository.GetListAsync<Notification>(x => x.UserId.ToLower() == userId.ToLower() && x.IsActive && x.IsCompleted == false);
            var dtoList = await dbData.BindDtoList(mapper, BindEmployeeData);
            return dtoList.ToList();
        }

        #region PrivateMethods
        private async Task<EmployeeDto> BindEmployeeData(string userId)
        {
            var employee = await requestQueueService.GetResponseAsync<string, EmployeeDto>(userId);
            return employee;
        }
        #endregion
    }
}