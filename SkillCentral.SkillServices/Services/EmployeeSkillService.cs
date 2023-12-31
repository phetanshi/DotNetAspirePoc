using AutoMapper;
using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Data.DbModels;
using SkillCentral.SkillServices.Utils;

namespace SkillCentral.SkillServices.Services
{
    public class EmployeeSkillService(IRepository repository, IMapper mapper, ILogger<EmployeeSkillService> logger, IMqRequestService requestQueueService, IMqPubSubService pubSubQueueService, ISkillService skillService, IHttpContextAccessor context) : ServiceBase(context), IEmployeeSkillService
    {
        public async Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill)
        {
            if (skill is null)
                throw new ArgumentNullException(GlobalConstants.EMPLOYEE_SKILL_OBJ_NULL);
            
            var dbEmpSkill = mapper.Map<EmployeeSkill>(skill);
            dbEmpSkill.DateCreated = DateTime.Now;
            dbEmpSkill.CreatedUserId = GetLoginUserId();
            dbEmpSkill = await repository.CreateAsync(dbEmpSkill);
            var dto = await BindEmployeeSkillData(dbEmpSkill);

            await SendNotification(dto.Employee?.UserId ?? "", SkillServiceConstants.EMPLOYEE_SKILL_ADDED, MQConstants.EMPLOYEE_SKILL_ADDED_ROUTE_KEY);
            return dto;
        }

        public async Task<List<EmployeeSkillDto>> GetAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException(GlobalConstants.USER_ID_NULL);

            var data = (await repository.GetListAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower() && x.IsActive))?.ToList();
            if (data is not null && data.Count > 0)
            {
                var empDtoList = new List<EmployeeSkillDto>();
                foreach(var item in data)
                {
                    var dto = await BindEmployeeSkillData(item);
                    empDtoList.Add(dto);
                }
                return empDtoList;
            }
            return new List<EmployeeSkillDto>();
        }

        public async Task<EmployeeSkillDto> GetAsync(string userId, int skillId)
        {
            if (userId is null || skillId  == 0)
                throw new ArgumentNullException($"{GlobalConstants.USER_ID_NULL} or {GlobalConstants.SKILL_ID_ZERO}");

            var data = await repository.GetSingleAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower() && x.IsActive);
            if (data is not null)
            {
                return await BindEmployeeSkillData(data);
            }
            return null;
        }

        public async Task<bool> RemoveSkillAsync(string userId, int skillId)
        {
            if (userId is null || skillId == 0)
                throw new ArgumentNullException(GlobalConstants.USER_ID_NULL);

            var record = await repository.GetSingleAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower() && x.SkillId == skillId);
            record.IsActive = false;
            record.DateUpdated = DateTime.UtcNow;
            record.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(record);
            bool isTrue = count > 0;

            if (isTrue)
            {
                await SendNotification(userId, SkillServiceConstants.EMPLOYEE_SKILL_DELETED, MQConstants.EMPLOYEE_SKILL_DELETED_ROUTE_KEY);
            }

            return isTrue;
        }
        public async Task<bool> RemoveSkillsAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException(GlobalConstants.USER_ID_NULL);

            var records = (await repository.GetListAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower())).ToList();
            if (records is not null && records.Any())
            {
                foreach (var record in records)
                {
                    record.IsActive = false;
                    record.DateUpdated = DateTime.UtcNow;
                    record.UpdatedUserId = GetLoginUserId();
                    await repository.UpdateAsync(record);
                }
            }

            await SendNotification(userId, SkillServiceConstants.EMPLOYEE_ALL_SKILL_DELETED, MQConstants.EMPLOYEE_SKILL_DELETED_ROUTE_KEY);
            return true;
        }

        #region PrivateMethods
        private async Task<EmployeeSkillDto> BindEmployeeSkillData(EmployeeSkill item)
        {
            EmployeeSkillDto dto = mapper.Map<EmployeeSkillDto>(item);
            dto.Employee = await requestQueueService.GetResponseAsync<string, EmployeeDto>(item.UserId);
            dto.Skill = await skillService.GetAsync(item.SkillId);
            return dto;
        }

        private async Task SendNotification(string userId, string notification, string routeKey, bool isUser = true)
        {
            NotificationCreateDto notificationDto = new NotificationCreateDto();
            notificationDto.UserId = userId;
            notificationDto.Notification = notification;
            if (!isUser)
            {
                notificationDto.IsAdmin = true;
                notificationDto.IsSupport = true;
            }
            else
            {
                notificationDto.IsAdmin = false;
                notificationDto.IsSupport = false;
            }
            await pubSubQueueService.PublishTopicAsync(notificationDto, routeKey);
        }
        #endregion
    }
}
