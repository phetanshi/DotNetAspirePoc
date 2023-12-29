﻿using Ps.RabbitMq.Client;
using RabbitMQ.Client;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Services;

namespace SkillCentral.SkillServices.Contracts
{
    public class EmployeeMQContract(IServiceProvider serviceProvider, IConnection connection, IMqPubSubService rabbitPubSubService, ILogger<EmployeeMQContract> logger)
    {
        private readonly IEmployeeSkillService _empSkillService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmployeeSkillService>();

        public async Task InvokeAsync()
        {
            await rabbitPubSubService.ConsumeTopicAsync<EmployeeDto>(MQConstants.EMPLOYEE_DELETE_ROUTE_KEY, async (emp) =>
            {
                if (emp is null || emp.UserId is null)
                    return;
                await _empSkillService.RemoveSkillsAsync(emp.UserId);
            });
        }
    }
}
