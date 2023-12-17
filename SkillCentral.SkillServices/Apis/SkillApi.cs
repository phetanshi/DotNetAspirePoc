namespace SkillCentral.SkillServices.Apis;
public static class SkillApi
{
    public static void MapSkillApiEndpoints(this WebApplication? app)
    {
        app.MapGet("/skillsvc/skills", async () =>
        {
            return await Task.FromResult(0);
        })
        .WithName("GetSkills")
        .WithOpenApi();
    }
}
