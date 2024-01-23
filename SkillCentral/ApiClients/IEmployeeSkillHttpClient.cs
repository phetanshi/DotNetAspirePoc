using SkillCentral.Dtos;

namespace SkillCentral.ApiClients
{
    public interface IEmployeeSkillHttpClient
    {
        public Task<List<EmployeeSkillDto>> GetEmployeeSkillsAsync(string userId);
        public Task<EmployeeSkillDto> AddEmployeeSkillAsync(EmployeeSkillCreateDto dto);
    }
}
