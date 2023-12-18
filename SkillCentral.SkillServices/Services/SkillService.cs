using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Data.DbModels;

namespace SkillCentral.SkillServices.Services
{
    public class SkillService(IRepository repository, IMapper mapper, ILogger<SkillService> logger, IHttpContextAccessor context) : ServiceBase(context), ISkillService
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

            return mapper.Map<SkillDto>(dbSkill);
        }

        public async Task<bool> DeleteAsync(int skillId)
        {
            var skill = await repository.GetSingleAsync<Skill>(skillId);
            skill.IsActive = false;
            skill.DateUpdated = DateTime.UtcNow;
            skill.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(skill);
            return count > 0;
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
                return mapper.Map<SkillDto>(updatedData);

            return null;
        }
    }
}
