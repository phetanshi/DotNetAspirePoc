using Microsoft.AspNetCore.Mvc;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Services;

namespace SkillCentral.SkillServices.Apis
{
    public static class SkillApis
    {
        public static void MapSkillApiEndpoints(this WebApplication? app)
        {
            app.MapGet("/skillsvc/skills", async (ISkillService skillService) =>
            {
                var data = await skillService.GetAsync();
                ApiResponse<List<SkillDto>> apiResponse = new ApiResponse<List<SkillDto>>();

                apiResponse.IsSuccess = true;
                apiResponse.Message = data is null ? "Something went wrong!" : "Skill details were found!";
                apiResponse.Payload = data;
                return apiResponse;
            })
            .WithName("GetSkills")
            .WithOpenApi();

            app.MapGet("/skillsvc/skillbyid", async (ISkillService skillService, [FromQuery] int skillId) =>
            {
                var data = await skillService.GetAsync(skillId);
                ApiResponse<SkillDto> apiResponse = new ApiResponse<SkillDto>();

                apiResponse.IsSuccess = true;
                apiResponse.Message = data is null ? "Details were not found!" : "Skill details were found!";
                apiResponse.Payload = data;
                return apiResponse;
            })
            .WithName("GetSkillById")
            .WithOpenApi();

            app.MapPost("/skillsvc/createskill", async (ISkillService skillService, [FromBody] SkillCreateDto skill) =>
            {
                var data = await skillService.CreateAsync(skill);
                ApiResponse<SkillDto> apiResponse = new ApiResponse<SkillDto>();

                apiResponse.IsSuccess = data is not null ? true : false;
                apiResponse.Message = data is null ? "Something went wrong!" : "New skill have been created!";
                apiResponse.Payload = data;
                return apiResponse;
            })
            .WithName("CreateSkill")
            .WithOpenApi();

            app.MapPut("/skillsvc/updateskill", async (ISkillService skillService, [FromBody] SkillDto updatedSkill) =>
            {
                var data = await skillService.UpdateAsync(updatedSkill);
                ApiResponse<SkillDto> apiResponse = new ApiResponse<SkillDto>();

                apiResponse.IsSuccess = data is not null ? true : false;
                apiResponse.Message = data is null ? "Something went wrong!" : "Skill have been updated successfully!";
                apiResponse.Payload = data;
                return apiResponse;
            })
            .WithName("UpdateSkill")
            .WithOpenApi();

            app.MapDelete("/skillsvc/deleteskill", async (ISkillService skillService, [FromQuery] int skillId) =>
            {
                var data = await skillService.DeleteAsync(skillId);
                ApiResponse<bool> apiResponse = new ApiResponse<bool>();

                apiResponse.IsSuccess = data;
                apiResponse.Message = !data ? "Something went wrong!" : "Skill have been deleted successfully!";
                apiResponse.Payload = data;
                return apiResponse;
            })
            .WithName("DeleteSkill")
            .WithOpenApi();
        }
    }
}
