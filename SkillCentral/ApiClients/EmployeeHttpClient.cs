
using SkillCentral.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkillCentral.ApiClients;

public class EmployeeHttpClient(HttpClient http, ILogger<EmployeeHttpClient> logger) : ClientBase(http, logger), IEmployeeHttpClient
{
    public async Task<List<EmployeeDto>> GetEmployee()
    {
        try
        {
            var response = await Http.GetAsync("/employeesvc/employees");
            var data = await response.Content.ReadFromJsonAsync<ApiResponse<List<EmployeeDto>>>();
            return data?.Payload ?? new List<EmployeeDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while fetching employee list data from api");
        }
        return new List<EmployeeDto>();
    }

    public async Task<EmployeeDto> GetEmployee(string userId)
    {
        var data = await http.GetFromJsonAsync<ApiResponse<EmployeeDto>>($"/employeesvc/employeebyid?userId={userId}");
        return data?.Payload ?? default;
    }
}
