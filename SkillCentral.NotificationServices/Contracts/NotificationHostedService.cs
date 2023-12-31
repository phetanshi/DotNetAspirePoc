using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Services;
using SkillCentral.NotificationServices.Utils;

namespace SkillCentral.NotificationServices.Contracts
{
    public class NotificationHostedService(IServiceProvider serviceProvider, IMqPubSubService rabbitPubSubService, ILogger<NotificationHostedService> logger) : BackgroundService
    {
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await rabbitPubSubService.ConsumeTopicAsync<NotificationCreateDto>(MQConstants.EMPLOYEE_SKILL_LIKE_ROUTE_KEY, async (dto) =>
            {
                //This needs to be initialze here only as the hosted server is a singleton class
                INotificationService _notificationService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<INotificationService>();
                if (dto is null || string.IsNullOrEmpty(dto.UserId))
                    return;
                dto.IsCompleted = false;
                await _notificationService.CreateAsync(dto);
            });

            await rabbitPubSubService.ConsumeTopicAsync<NotificationCreateDto>(MQConstants.SKILL_LIKE_ROUTE_KEY, async (dto) =>
            {
                if (dto is null)
                    return;

                //This needs to be initialze here only as the hosted server is a singleton class
                INotificationService _notificationService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<INotificationService>();

                dto.IsAdmin = true;
                dto.IsSupport = true;
                dto.IsCompleted = false;
                await _notificationService.CreateAsync(dto);
            });

            await rabbitPubSubService.ConsumeTopicAsync<NotificationCreateDto>(MQConstants.EMPLOYEE_LIKE_ROUTE_KEY, async (dto) =>
            {
                if (dto is null)
                    return;

                //This needs to be initialze here only as the hosted server is a singleton class
                INotificationService _notificationService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<INotificationService>();
                dto.IsCompleted = false;
                await _notificationService.CreateAsync(dto);
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
