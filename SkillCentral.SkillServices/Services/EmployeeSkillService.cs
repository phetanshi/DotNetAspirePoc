using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Data.DbModels;

namespace SkillCentral.SkillServices.Services
{
    public class EmployeeSkillService(IRepository repository, IMapper mapper, ILogger<EmployeeSkillService> logger, IHttpContextAccessor context) : ServiceBase(context), IEmployeeSkillService
    {
        public async Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill)
        {
            if (skill is null)
                throw new ArgumentNullException("employee skill input object cannot be null");
            
            var dbEmpSkill = mapper.Map<EmployeeSkill>(skill);
            dbEmpSkill.DateCreated = DateTime.Now;
            dbEmpSkill.CreatedUserId = GetLoginUserId();
            dbEmpSkill = await repository.CreateAsync(dbEmpSkill);

            return mapper.Map<EmployeeSkillDto>(dbEmpSkill);
        }

        public async Task<List<EmployeeSkillDto>> GetAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException("userid cannot be null");

            var data = await repository.GetListAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower());
            if (data is not null)
                return mapper.Map<List<EmployeeSkillDto>>(data);
            return new List<EmployeeSkillDto>();
        }

        public async Task<bool> RemoveSkill(string userId, int skillId)
        {
            if (userId is null || skillId == 0)
                throw new ArgumentNullException("userid cannot be null");

            var record = await repository.GetSingleAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower() && x.SkillId == skillId);
            record.IsActive = false;
            record.DateUpdated = DateTime.UtcNow;
            record.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(record);
            return count > 0;
        }
    }
}
