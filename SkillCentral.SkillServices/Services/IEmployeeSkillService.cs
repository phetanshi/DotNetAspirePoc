using SkillCentral.Dtos;

namespace SkillCentral.SkillServices.Services
{
    public interface IEmployeeSkillService
    {
        Task<List<EmployeeSkillDto>> GetAsync(string userId);
        Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill);
        Task<bool> RemoveSkillAsync(string userId, int skillId);
        Task<bool> RemoveSkillsAsync(string userId);
    }
}
