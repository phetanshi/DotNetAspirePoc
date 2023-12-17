using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Data;

namespace SkillCentral.SkillServices.Services
{
    public class SkillService(SkillDbContext database, IMapper mapper, ILogger<SkillService> logger) : ISkillService
    {
        public Task<SkillDto> CreateAsync(SkillCreateDto skill)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int skillId)
        {
            throw new NotImplementedException();
        }

        public Task<SkillDto> GetAsync(int skillId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SkillDto>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SkillDto> UpdateAsync(SkillDto updatedSkill)
        {
            throw new NotImplementedException();
        }
    }
}
