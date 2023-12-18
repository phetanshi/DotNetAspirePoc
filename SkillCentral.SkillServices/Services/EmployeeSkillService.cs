using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillCentral.Dtos;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Data;
using SkillCentral.SkillServices.Data.DbModels;

namespace SkillCentral.SkillServices.Services
{
    public class EmployeeSkillService(IRepository repository, IMapper mapper, ILogger<EmployeeSkillService> logger) : IEmployeeSkillService
    {
        public async Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill)
        {
            if (skill is null)
                throw new ArgumentNullException("employee skill input object cannot be null");
            var e = await repository.CreateWithMapperAsync<EmployeeSkillCreateDto, EmployeeSkill>(skill);
            if (e is not null)
                return mapper.Map<EmployeeSkillDto>(e);
            return null;
        }

        public async Task<List<EmployeeSkillDto>> GetAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException("userid cannot be null");

            var data = await repository.GetListAsync<EmployeeSkill>(x => string.Equals(x.UserId, userId, StringComparison.OrdinalIgnoreCase));
            if (data is not null)
                return mapper.Map<List<EmployeeSkillDto>>(data);
            return new List<EmployeeSkillDto>();
        }

        public async Task<bool> RemoveSkill(string userId, int skillId)
        {
            if (userId is null || skillId == 0)
                throw new ArgumentNullException("userid cannot be null");

            var record = await repository.GetSingleAsync<EmployeeSkill>(x => string.Equals(x.UserId, userId, StringComparison.OrdinalIgnoreCase) && x.SkillId == skillId);
            record.IsActive = false;
            record.DateUpdated = DateTime.UtcNow;
            record.UpdatedUserId = "deleteuser";
            int count = await repository.UpdateAsync(record);
            return count > 0;
        }
    }
}
