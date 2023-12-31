using Ps.RabbitMq.Client;
using RabbitMQ.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Services;

namespace SkillCentral.EmployeeServices.Contracts
{
    public class EmployeeHostedService(IServiceProvider serviceProvider, IMqRequestService requestQueueService, ILogger<EmployeeHostedService> logger) : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await requestQueueService.GetRequestAsync<string, EmployeeDto>(typeof(EmployeeDto).FullName, userId =>
            {
                IEmployeeService _employeeService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeService>();
                return _employeeService.GetAsync(userId).Result;
            });
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
