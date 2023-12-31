using AutoMapper;
using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Data.DbModels;
using SkillCentral.SkillServices.Utils;

namespace SkillCentral.SkillServices.Services
{
    public class SkillService(IRepository repository, IMapper mapper, ILogger<SkillService> logger, IHttpContextAccessor context, IMqPubSubService pubSubQueueService) : ServiceBase(context), ISkillService
    {
        public async Task<SkillDto> CreateAsync(SkillCreateDto skill)
        {
            if(skill is null)
                throw new ArgumentNullException("skill input object cannot be null");


            var dbSkill = mapper.Map<Skill>(skill);
            dbSkill.DateCreated = DateTime.Now;
            dbSkill.CreatedUserId = GetLoginUserId();
            dbSkill = await repository.CreateAsync(dbSkill);

            if (skill is null)
                return null;

            var dto = mapper.Map<SkillDto>(dbSkill);
            await SendNotification(userId: "", notification: SkillServiceConstants.SKILL_ADDED, routeKey: MQConstants.SKILL_ADDED_ROUTE_KEY);
            return dto;
        }

        public async Task<bool> DeleteAsync(int skillId)
        {
            var skill = await repository.GetSingleAsync<Skill>(skillId);
            skill.IsActive = false;
            skill.DateUpdated = DateTime.UtcNow;
            skill.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(skill);
            bool isDeleted = count > 0;
            await SendNotification(userId: "", notification: SkillServiceConstants.SKILL_DELETED, routeKey: MQConstants.SKILL_DELETED_ROUTE_KEY);
            return isDeleted;
        }

        public async Task<SkillDto> GetAsync(int skillId)
        {
            var skill = await repository.GetSingleAsync<Skill>(skillId);

            if (skill is null)
                return null;

            return mapper.Map<SkillDto>(skill);
        }

        public async Task<List<SkillDto>> GetAsync()
        {
            var data = await repository.GetListAsync<Skill>(s => s.IsActive);
            if(data is null)
                return new List<SkillDto>();
            return mapper.Map<List<SkillDto>>(data);
        }

        public async Task<SkillDto> UpdateAsync(SkillDto updatedSkill)
        {
            var skill = await repository.GetSingleAsync<Skill>(s => s.Id == updatedSkill.Id);
            if (skill is null)
                return null;
            var updatedData = mapper.Map(updatedSkill, skill);

            if (updatedData is null)
                return null;

            updatedData.DateUpdated = DateTime.UtcNow;
            updatedData.UpdatedUserId = GetLoginUserId();

            int count = await repository.UpdateAsync(updatedData);
            if (count > 0)
            {
                await SendNotification(userId: "", notification: SkillServiceConstants.SKILL_UPDATED, routeKey: MQConstants.SKILL_DELETED_ROUTE_KEY);
                return mapper.Map<SkillDto>(updatedData);
            }
            return null;
        }

        #region PrivateMethods
        private async Task SendNotification(string userId, string notification, string routeKey)
        {
            NotificationCreateDto notificationDto = new NotificationCreateDto();
            notificationDto.UserId = userId;
            notificationDto.Notification = notification;
            await pubSubQueueService.PublishTopicAsync(notificationDto, routeKey);
        }
        #endregion
    }
}
