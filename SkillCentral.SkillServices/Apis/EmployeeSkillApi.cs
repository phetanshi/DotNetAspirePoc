using SkillCentral.Dtos;
using SkillCentral.SkillServices.Services;

namespace SkillCentral.SkillServices.Apis;
public static class EmployeeSkillApi
{
    public static void MapEmployeeSkillApiEndpoints(this WebApplication? app)
    {
        app.MapGet("/skillsvc/employeeskills", async (IEmployeeSkillService employeeSkillService, string userId) =>
        {
            var data = await employeeSkillService.GetAsync(userId);

            ApiResponse<List<EmployeeSkillDto>> apiResponse = new ApiResponse<List<EmployeeSkillDto>>();

            apiResponse.IsSuccess = data is not null ? true : false;
            apiResponse.Message = data is null ? "Something went wrong!" : "";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("GetEmployeeSkills")
        .WithOpenApi();

        app.MapPost("/skillsvc/addemployeeskill", async (IEmployeeSkillService employeeSkillService, EmployeeSkillCreateDto skill) =>
        {
            var data = await employeeSkillService.CreateAsync(skill);

            ApiResponse<EmployeeSkillDto> apiResponse = new ApiResponse<EmployeeSkillDto>();

            apiResponse.IsSuccess = data is not null ? true : false;
            apiResponse.Message = data is null ? "Something went wrong!" : "";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("AddEmployeeSkill")
        .WithOpenApi();
    }
}
