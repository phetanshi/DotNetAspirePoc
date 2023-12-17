namespace SkillCentral.EmployeeServices.Apis;

public static class EmployeeApi
{
    public static void MapEmployeeApiEndpoints(this WebApplication? app)
    {
        app.MapGet("/employeesvc/employees", async () =>
        {
            return await Task.FromResult(0);
        })
        .WithName("GetEmployees")
        .WithOpenApi();
    }
}
