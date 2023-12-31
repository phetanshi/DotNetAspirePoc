using Ps.RabbitMq.Client;
using RabbitMQ.Client;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Services;

namespace SkillCentral.SkillServices.Contracts
{
    public class EmployeeSkillHostedService(IServiceProvider serviceProvider, IMqPubSubService rabbitPubSubService, ILogger<EmployeeSkillHostedService> logger) : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await rabbitPubSubService.ConsumeTopicAsync<EmployeeDto>(MQConstants.EMPLOYEE_DELETE_ROUTE_KEY, async (emp) =>
            {
                if (emp is null || emp.UserId is null)
                    return;

                IEmployeeSkillService _empSkillService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeSkillService>();
                await _empSkillService.RemoveSkillsAsync(emp.UserId);
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
