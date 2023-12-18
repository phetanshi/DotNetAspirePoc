using SkillCentral.Dtos;

namespace SkillCentral.SkillServices.Services
{
    public interface IEmployeeSkillService
    {
        Task<List<EmployeeSkillDto>> GetAsync(string userId);
        Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill);
        Task<bool> RemoveSkill(string userId, int skillId);
    }
}
