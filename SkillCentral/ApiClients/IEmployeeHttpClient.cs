using SkillCentral.Dtos;

namespace SkillCentral.ApiClients;

public interface IEmployeeHttpClient
{
    Task<List<EmployeeDto>> GetEmployee();
    Task<EmployeeDto> GetEmployee(string userId);
}
