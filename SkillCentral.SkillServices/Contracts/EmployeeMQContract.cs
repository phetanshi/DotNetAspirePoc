using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Services;

namespace SkillCentral.SkillServices.Contracts
{
    public class EmployeeMQContract(IMqPubSubService pubSubQueueService,
        IServiceProvider serviceProvider,
        ILogger<EmployeeMQContract> logger)
    {
        IEmployeeSkillService empSkillService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeSkillService>();
        public async Task InvokeAsync()
        {
            await pubSubQueueService.ConsumeTopicAsync<EmployeeDto>(MQConstants.DELETE_EMPLOYEE, async (emp) =>
            {
                if (emp is null || emp.UserId is null)
                    return;
                await empSkillService.RemoveSkillsAsync(emp.UserId);
            });
        }
    }
}
