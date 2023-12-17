namespace SkillCentral.NotificationServices.Apis;

public static class NotificationApi
{
    public static void MapNotificationApiEndpoints(this WebApplication? app)
    {
        app.MapGet("/notificationsvc/notifications", async (string userId) =>
        {
            return await Task.FromResult(0);
        })
        .WithName("GetNotification")
        .WithOpenApi();
    }
}
