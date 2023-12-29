using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Services;
using SkillCentral.NotificationServices.Utils;

namespace SkillCentral.NotificationServices.Contracts
{
    public class NotificationHostedService(IServiceProvider serviceProvider, IMqPubSubService rabbitPubSubService, ILogger<NotificationHostedService> logger) : BackgroundService
    {
        private readonly INotificationService _notificationService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<INotificationService>();
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await rabbitPubSubService.ConsumeTopicAsync<NotificationCreateDto>(MQConstants.EMPLOYEE_SKILL_LIKE_ROUTE_KEY, async (dto) =>
            {
                if (dto is null || string.IsNullOrEmpty(dto.UserId))
                    return;
                await _notificationService.CreateAsync(dto);
            });

            await rabbitPubSubService.ConsumeTopicAsync<NotificationCreateDto>(MQConstants.SKILL_LIKE_ROUTE_KEY, async (dto) =>
            {
                if (dto is null)
                    return;
                await _notificationService.CreateAsync(dto);
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
