using Ps.RabbitMq.Client;
using RabbitMQ.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Services;

namespace SkillCentral.EmployeeServices.Contracts;

public class EmployeeMQContract(IServiceProvider serviceProvider, IConnection connection, IMqRequestService requestQueueService, ILogger<EmployeeMQContract> logger)
{
    public async Task HandleGetEmployeeRequest()
    {
        await requestQueueService.GetRequestAsync<string, EmployeeDto>(typeof(EmployeeDto).FullName, userId =>
        {
            IEmployeeService _employeeService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeService>();
            return _employeeService.GetAsync(userId).Result;
        });
    }
}
