
using SkillCentral.Dtos;
using System.Net.Http.Json;
using System.Text;

namespace SkillCentral.ApiClients;

public class SkillHttpClient(HttpClient http, ILogger<SkillHttpClient> logger) : ClientBase(http, logger), ISkillHttpClient
{
    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        List<SkillDto> data = new List<SkillDto>();
        try
		{
            data = await GetListAsync<SkillDto>("/skillsvc/skills");
            if(data is null)
            {
                data = new List<SkillDto>();
            }
        }
		catch (Exception ex)
		{
            logger.LogError(ex, "Error while fetching skill list");
        }
        return data;
    }

    public async Task<SkillDto> CreateSkillAsync(SkillCreateDto skillDto)
    {
        SkillDto dto = null;

        try
        {
            var response = await Http.PostAsJsonAsync("/skillsvc/createskill", skillDto);
            return await ReadPostResponseAsync<SkillDto>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating new skill");
        }
        return dto;
    }
    public async Task DeleteSkillAsync(int skillId)
    {
        try
        {
            var response =  await Http.DeleteAsync($"/skillsvc/deleteskill?skillId={skillId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Error occurred while deleting skill. Api call returned error code");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting a skill");
        }
    }
}
