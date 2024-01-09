
using SkillCentral.Dtos;
using System.Net.Http.Json;
using System.Text;

namespace SkillCentral.ApiClients;

public class SkillHttpClient(HttpClient http, ILogger<SkillHttpClient> logger) : ClientBase(http, logger), ISkillHttpClient
{
    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        try
		{
            var response = await Http.GetAsync("/skillsvc/skills");
            return await ReadSkillListData(response);

        }
		catch (Exception ex)
		{
            logger.LogError(ex, "Error while fetching skill list");
        }
		return new List<SkillDto>();
    }

    public async Task<SkillDto> CreateSkillAsync(SkillCreateDto skillDto)
    {
        SkillDto dto = null;

        try
        {
            var response = await Http.PostAsJsonAsync("/skillsvc/createskill", skillDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<SkillDto>>();
                if (!result.IsSuccess)
                    throw new Exception("Something went wrong while creating a new skill");
                return result.Payload;
            }
            else
            {
                var result = (await response?.Content?.ReadFromJsonAsync<ApiResponse<SkillDto>>())?.Message ?? "Something went wrong while creating a new skill. API call returned error code.";
                throw new Exception(result);
            }
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

    private async Task<List<SkillDto>> ReadSkillListData(HttpResponseMessage? response)
    {
        var data = await response.Content.ReadFromJsonAsync<ApiResponse<List<SkillDto>>>();
        return data?.Payload ?? new List<SkillDto>();
    }

    
}
