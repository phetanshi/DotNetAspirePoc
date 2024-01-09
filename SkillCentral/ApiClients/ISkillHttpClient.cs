using SkillCentral.Dtos;

namespace SkillCentral.ApiClients;

public interface ISkillHttpClient
{
    Task<List<SkillDto>> GetSkillsAsync();
    Task<SkillDto> CreateSkillAsync(SkillCreateDto skillDto);
    Task DeleteSkillAsync(int skillId);
}
