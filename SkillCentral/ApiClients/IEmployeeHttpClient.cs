using SkillCentral.Dtos;

namespace SkillCentral.ApiClients;

public interface IEmployeeHttpClient
{
    Task<List<EmployeeDto>> GetEmployeeAsync();
    Task<EmployeeDto> GetEmployeeAsync(string userId);
    Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto emp);
}
