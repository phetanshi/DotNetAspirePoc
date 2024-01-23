
using SkillCentral.Dtos;

namespace SkillCentral.ApiClients
{
    public class EmployeeSkillHttpClient(HttpClient http, ILogger<EmployeeSkillHttpClient> logger) : ClientBase(http, logger), IEmployeeSkillHttpClient
    {
        public async Task<List<EmployeeSkillDto>> GetEmployeeSkillsAsync(string userId)
        {
            try
            {
                var data = await GetListAsync<EmployeeSkillDto>($"/skillsvc/employeeskills?userId={userId}");
                return data;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while fetching employee list data from api");
            }
            return new List<EmployeeSkillDto>();
        }

        public async Task<EmployeeSkillDto> AddEmployeeSkillAsync(EmployeeSkillCreateDto dto)
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/skillsvc/addemployeeskill", dto);
                return await ReadPostResponseAsync<EmployeeSkillDto>(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while adding a new skill for a given employee skill set");
            }
            return null;
        }
    }
}
