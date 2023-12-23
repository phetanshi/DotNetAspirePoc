using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data.DbModels;
using SkillCentral.EmployeeServices.Services;
using SkillCentral.ServiceDefaults;

namespace SkillCentral.EmployeeServices.Contracts
{
    public class EmployeeMQContract(IMQUtil queueService, IEmployeeService employeeService, ILogger<EmployeeMQContract> logger)
    {
        public void HandleGetEmployeeRequest()
        {
            queueService.GetRequest<string, EmployeeDto>(typeof(EmployeeDto).FullName, userId => employeeService.GetAsync(userId).Result);
        }
    }
}
