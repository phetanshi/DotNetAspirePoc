using Microsoft.AspNetCore.Mvc;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Services;

namespace SkillCentral.NotificationServices.Apis;

public static class NotificationApi
{
    public static void MapNotificationApiEndpoints(this WebApplication? app)
    {
        app.MapGet("/notificationsvc/notifications", async (INotificationService notificaitonService, string userId) =>
        {
            var data = await notificaitonService.GetAsync(userId);
            ApiResponse<List<NotificationDto>> apiResponse = new ApiResponse<List<NotificationDto>>();

            apiResponse.IsSuccess = true;
            apiResponse.Message = data is null ? "Data did not found!" : "";
            apiResponse.Payload = data;

            return apiResponse;
        })
        .WithName("GetAllUserNotifications")
        .WithOpenApi();

        app.MapGet("/notificationsvc/adminnotifications", async (INotificationService notificaitonService) =>
        {
            var data = await notificaitonService.GetAdminAsync();
            ApiResponse<List<NotificationDto>> apiResponse = new ApiResponse<List<NotificationDto>>();

            apiResponse.IsSuccess = true;
            apiResponse.Message = data is null ? "Data did not found!" : "";
            apiResponse.Payload = data;

            return apiResponse;
        })
        .WithName("GetAdminNotifications")
        .WithOpenApi();

        app.MapGet("/notificationsvc/supportnotifications", async (INotificationService notificaitonService) =>
        {
            var data = await notificaitonService.GetSupportAsync();
            ApiResponse<List<NotificationDto>> apiResponse = new ApiResponse<List<NotificationDto>>();

            apiResponse.IsSuccess = true;
            apiResponse.Message = data is null ? "Data did not found!" : "";
            apiResponse.Payload = data;

            return apiResponse;
        })
        .WithName("GetSupportNotifications")
        .WithOpenApi();

        app.MapPost("/notificationsvc/addnotification", async (INotificationService notificaitonService, NotificationCreateDto notification) =>
        {
            var data = await notificaitonService.CreateAsync(notification);
            ApiResponse<NotificationDto> apiResponse = new ApiResponse<NotificationDto>();

            apiResponse.IsSuccess = data is not null ? true : false;
            apiResponse.Message = data is null ? "Something went wrong!" : "";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("GetNotificationDetails")
        .WithOpenApi();

        app.MapPut("/notificationsvc/markascompleted", async (INotificationService notificaitonService, [FromBody] int notificationId) =>
        {
            var data = await notificaitonService.CompletedAsync(notificationId);
            ApiResponse<NotificationDto> apiResponse = new ApiResponse<NotificationDto>();

            apiResponse.IsSuccess = data is not null ? true : false;
            apiResponse.Message = data is null ? "Something went wrong!" : "";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("MarkAsCompleted")
        .WithOpenApi();

        app.MapDelete("/notificationsvc/delete", async (INotificationService notificaitonService, [FromBody] int notificationId) =>
        {
            var data = await notificaitonService.DeleteAsync(notificationId);
            ApiResponse<bool> apiResponse = new ApiResponse<bool>();
            apiResponse.IsSuccess = data;
            apiResponse.Message = !data ? "Something went wrong!" : "";
            apiResponse.Payload = data;
            return apiResponse;
        })
        .WithName("DeleteNotification")
        .WithOpenApi();
    }
}
