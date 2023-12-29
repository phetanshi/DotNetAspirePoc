using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Services;

namespace SkillCentral.EmployeeServices.Contracts;

public class EmployeeMQContract(IMqRequestService requestQueueService, IEmployeeService employeeService, ILogger<EmployeeMQContract> logger)
{
    public async Task HandleGetEmployeeRequest()
    {
        await requestQueueService.GetRequestAsync<string, EmployeeDto>(userId => employeeService.GetAsync(userId).Result, typeof(EmployeeDto).FullName);
    }
}
