using Ps.RabbitMq.Client;
using RabbitMQ.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Services;

namespace SkillCentral.EmployeeServices.Contracts;

public class EmployeeMQContract(IServiceProvider serviceProvider, IConnection connection, IMqRequestService requestQueueService, ILogger<EmployeeMQContract> logger)
{
    private readonly IEmployeeService _employeeService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeService>();

    public async Task HandleGetEmployeeRequest()
    {
        await requestQueueService.GetRequestAsync<string, EmployeeDto>(userId => _employeeService.GetAsync(userId).Result, typeof(EmployeeDto).FullName);
    }
}
