using Ps.RabbitMq.Client;
using RabbitMQ.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Services;

namespace SkillCentral.EmployeeServices.Contracts
{
    public class EmployeeHostedService(IServiceProvider serviceProvider, IMqRequestService requestQueueService, ILogger<EmployeeHostedService> logger) : BackgroundService
    {
        private readonly IEmployeeService _employeeService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeService>();
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await requestQueueService.GetRequestAsync<string, EmployeeDto>(userId => _employeeService.GetAsync(userId).Result, typeof(EmployeeDto).FullName);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
