using SkillCentral.Dtos;

namespace SkillCentral.SkillServices.Services
{
    public interface ISkillService
    {
        Task<SkillDto> GetAsync(int skillId);
        Task<List<SkillDto>> GetAsync();
        Task<SkillDto> CreateAsync(SkillCreateDto skill);
        Task<SkillDto> UpdateAsync(SkillDto updatedSkill);
        Task<bool> DeleteAsync(int skillId);
    }
}
