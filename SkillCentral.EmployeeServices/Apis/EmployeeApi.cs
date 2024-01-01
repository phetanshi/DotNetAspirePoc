using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Services;

namespace SkillCentral.EmployeeServices.Apis;

public static class EmployeeApi
{
    public static void MapEmployeeApiEndpoints(this WebApplication? app)
    {
        app.MapGet("/employeesvc/employees", async (IEmployeeService employeeService) =>
        {
            var data = await employeeService.GetAsync();
            ApiResponse<List<EmployeeDto>> apiResponse = new ApiResponse<List<EmployeeDto>>();
            apiResponse.IsSuccess = true;
            apiResponse.Message = data is null ? "Something went wrong!" : "Employee list found!";
            apiResponse.Payload = data;

            return apiResponse;
        })
        .WithName("GetEmployees")
        .WithOpenApi();

        app.MapGet("/employeesvc/employeebyid", async (IEmployeeService employeeService, [FromQuery]string userId) =>
        {
            var data = await employeeService.GetAsync(userId);
            ApiResponse<EmployeeDto> apiResponse = new ApiResponse<EmployeeDto>();
            apiResponse.IsSuccess = true;
            apiResponse.Message = data is null ? "Employee details were not found!" : "Employee details found!";
            apiResponse.Payload = data;

            return apiResponse;
        })
        .WithName("GetEmployeeById")
        .WithOpenApi();


        app.MapPost("/employeesvc/create", async (IEmployeeService employeeService, [FromBody] EmployeeCreateDto employee) =>
        {
            var data = await employeeService.CreateAsync(employee);
            ApiResponse<EmployeeDto> apiResponse = new ApiResponse<EmployeeDto>();
            if (data is not null)
            {
                apiResponse.Payload = data;
                apiResponse.IsSuccess = true;
                apiResponse.Message = GlobalConstants.EMPLOYEE_CREATION_SUCCESSFULL;
            }
            else
            {
                apiResponse.IsSuccess = false;
                apiResponse.Message = GlobalConstants.SOMETHING_WRONG;
            }
            return apiResponse;
        })
        .WithName("CreateEmployee")
        .WithOpenApi();

        app.MapPost("/employeesvc/processemployeebatch", async (IEmployeeService employeeService, IFileService fileService, [FromForm] IFormFile input) =>
        {
            List<EmployeeDto> data = new List<EmployeeDto>();
            try
            {
                data = await fileService.ReadEmployeeBatchFile<EmployeeDto>(input);
                await employeeService.ProcessEmployeeBatchAsync(data);
            }
            catch (Exception ex)
            {
            }
            ApiResponse<List<EmployeeDto>> apiResponse = new ApiResponse<List<EmployeeDto>>();
            if (data is not null && data.Count > 0)
            {
                apiResponse.Payload = data;
                apiResponse.IsSuccess = true;
                apiResponse.Message = GlobalConstants.EMPLOYEE_CREATION_SUCCESSFULL;
            }
            else
            {
                apiResponse.IsSuccess = false;
                apiResponse.Message = GlobalConstants.SOMETHING_WRONG;
            }
            return apiResponse;
        })
        .WithName("ProcessEmployeeBatch")
        .WithOpenApi()
        .DisableAntiforgery();


        app.MapPut("/employeesvc/update", async (IEmployeeService employeeService, [FromBody] EmployeeDto employee) =>
        {
            var data = await employeeService.UpdateAsync(employee);
            ApiResponse<EmployeeDto> apiResponse = new ApiResponse<EmployeeDto>();
            apiResponse.IsSuccess = data is not null ? true : false;
            apiResponse.Message = data is not null ? "Employee details updated successfully!" : "Something went wrong!";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("UpdateEmployee")
        .WithOpenApi();


        app.MapDelete("/employeesvc/delete", async (IEmployeeService employeeService, [FromBody] string userId) =>
        {
            var data = await employeeService.DeleteAsync(userId);
            ApiResponse<bool> apiResponse = new ApiResponse<bool>();
            apiResponse.IsSuccess = data;
            apiResponse.Message = data ? "Employee deleted successfully!" : "Something went wrong!";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("DeleteEmployee")
        .WithOpenApi();
    }
}
