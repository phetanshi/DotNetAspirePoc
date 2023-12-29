using AutoMapper;
using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data.DbModels;
using SkillCentral.EmployeeServices.Utils;
using SkillCentral.Repository;

namespace SkillCentral.EmployeeServices.Services
{
    public class EmployeeService(IRepository repository, IMapper mapper, ILogger<EmployeeService> logger, IHttpContextAccessor context, IMqRequestService requestQueueService, IMqPubSubService pubSubQueueService) : ServiceBase(context), IEmployeeService
    {
        public async Task<EmployeeDto> GetAsync(string userId)
        {
            var emp = await repository.GetSingleAsync<Employee>(x => x.UserId.ToLower() == userId.ToLower());
            if (emp is not null)
            {
                return mapper.Map<EmployeeDto>(emp);
            }
            return null;
        }

        public async Task<List<EmployeeDto>> GetAsync()
        {
            var lst = await repository.GetListAsync<Employee>(x => x.IsActive);
            if (lst is not null && lst.Count() > 0)
            {
                return mapper.Map<List<EmployeeDto>>(lst);
            }
            return new List<EmployeeDto>();
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto employee)
        {
            if (employee is null)
                throw new ArgumentNullException("employee input object cannot be null");

            var dbEmp = mapper.Map<Employee>(employee);
            dbEmp.DateCreated = DateTime.Now;
            dbEmp.CreatedUserId = GetLoginUserId();
            dbEmp = await repository.CreateAsync(dbEmp);

            return mapper.Map<EmployeeDto>(dbEmp);
        }
        public async Task<EmployeeDto> UpdateAsync(EmployeeDto updatedEmployee)
        {

            if (updatedEmployee is null)
                throw new ArgumentNullException("employee input object cannot be null");

            var emp = await repository.GetSingleAsync<Employee>(x => x.UserId.ToLower() == updatedEmployee.UserId.ToLower());

            emp.DateUpdated = DateTime.UtcNow;
            emp.UpdatedUserId = GetLoginUserId();
            emp.MergerEmployee(updatedEmployee);

            int count = await repository.UpdateAsync(emp);

            if (count > 0)
                return await GetAsync(updatedEmployee.UserId);
            return null;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException("userId cannot be null");

            var dbEmp = await repository.GetSingleAsync<Employee>(s => s.UserId.ToLower() == userId.ToLower() && s.IsActive);
            dbEmp.IsActive = false;
            dbEmp.DateUpdated = DateTime.Now;
            dbEmp.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(dbEmp);
            var isSuccess = count > 0;

            var empDto = mapper.Map<EmployeeDto>(dbEmp);

            if (isSuccess)
                pubSubQueueService.PublishTopicAsync(empDto, MQConstants.DELETE_EMPLOYEE);

            return isSuccess;
        }
    }
}
