
using SkillCentral.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkillCentral.ApiClients;

public class EmployeeHttpClient(HttpClient http, ILogger<EmployeeHttpClient> logger) : ClientBase(http, logger), IEmployeeHttpClient
{
    public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto emp)
    {
        try
        {
            var response = await Http.PostAsJsonAsync<EmployeeCreateDto>("/employeesvc/create", emp);
            return await ReadPostResponseAsync<EmployeeDto>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating employee");
        }
        return null;
    }

    public async Task<List<EmployeeDto>> GetEmployeeAsync()
    {
        try
        {
            return await GetListAsync<EmployeeDto>("/employeesvc/employees");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while fetching employee list data from api");
        }
        return new List<EmployeeDto>();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(string userId)
    {
        return await GetAsync<EmployeeDto>($"/employeesvc/employeebyid?userId={userId}");
    }
}
