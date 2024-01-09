
using SkillCentral.Dtos;
using System.Text;

namespace SkillCentral.ApiClients;

public class NotificationHttpClient(HttpClient http, ILogger<NotificationHttpClient> logger) : ClientBase(http, logger), INotificationHttpClient
{
    public async Task<List<NotificationDto>> GetAdminNotificationsAsync()
    {
        try
        {
            var response = await Http.GetAsync("/notificationsvc/adminnotifications");
            return await ReadNotificationListData(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while fetching admin notification list data from api");
        }
        return new List<NotificationDto>();
    }

    public async Task<List<NotificationDto>> GetEmployeeNotificationsAsync(string userId)
    {
        try
        {
            var response = await Http.GetAsync($"/notificationsvc/notifications?userId={userId}");
            return await ReadNotificationListData(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while fetching employee notification list data from api");
        }
        return new List<NotificationDto>();
    }

    public async Task MarkAsComplete(int notificationId)
    {
        try
        {
            StringContent content = new StringContent(notificationId.ToString(), Encoding.UTF8, "application/json");
            var response = await Http.PutAsync("/notificationsvc/markascompleted", content);
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content?.ReadFromJsonAsync<ApiResponse<bool>>();
                if (!result.IsSuccess)
                    throw new Exception(result.Message ?? "Something went wrong while marking the notification as completed!");
            }
            else
            {
                string error = await response.Content?.ReadAsStringAsync() ?? "";
                throw new Exception(error ?? "Something went wrong while marking the notification as completed. API call returned error code!");
            }
            logger.LogInformation("Successfully maked notification as completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while marking notification as completed");
        }
    }

    private async Task<List<NotificationDto>> ReadNotificationListData(HttpResponseMessage? response)
    {
        var data = await response.Content.ReadFromJsonAsync<ApiResponse<List<NotificationDto>>>();
        return data?.Payload ?? new List<NotificationDto>();
    }
}
