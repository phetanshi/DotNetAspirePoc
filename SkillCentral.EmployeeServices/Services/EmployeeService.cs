using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data.DbModels;
using SkillCentral.Repository;

namespace SkillCentral.EmployeeServices.Services
{
    public class EmployeeService(IRepository database, IMapper mapper, ILogger<EmployeeService> logger, IHttpContextAccessor context) : ServiceBase(context), IEmployeeService
    {
        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto employee)
        {
            if (employee is null)
                throw new ArgumentNullException("employee input object cannot be null");

            var dbEmp = mapper.Map<Employee>(employee);
            dbEmp.DateCreated = DateTime.Now;
            dbEmp.CreatedUserId = GetLoginUserId();
            dbEmp = await database.CreateAsync(dbEmp);

            return mapper.Map<EmployeeDto>(dbEmp);
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException("userId cannot be null");

            var dbEmp = await database.GetSingleAsync<Employee>(s => s.UserId.ToLower() == userId.ToLower());
            dbEmp.IsActive = false;
            dbEmp.DateUpdated = DateTime.Now;
            dbEmp.UpdatedUserId = GetLoginUserId();
            int count = await database.UpdateAsync(dbEmp);
            return count > 0;
        }

        public async Task<EmployeeDto> GetAsync(string userId)
        {
            var emp = await database.GetSingleAsync<Employee>(x => x.UserId.ToLower() == userId.ToLower());
            if (emp is not null)
            {
                return mapper.Map<EmployeeDto>(emp);
            }
            return null;
        }

        public async Task<List<EmployeeDto>> GetAsync()
        {
            var lst = await database.GetListAsync<Employee>(x => x.IsActive);
            if(lst is not null && lst.Count() > 0)
            {
                return mapper.Map<List<EmployeeDto>>(lst);
            }
            return new List<EmployeeDto>();
        }

        public async Task<EmployeeDto> UpdateAsync(EmployeeDto updatedEmployee)
        {

            if (updatedEmployee is null)
                throw new ArgumentNullException("employee input object cannot be null");

            var emp = await database.GetSingleAsync<Employee>(x => x.UserId.ToLower() == updatedEmployee.UserId.ToLower());
            mapper.Map(updatedEmployee, emp);
            emp.DateUpdated = DateTime.UtcNow;
            emp.UpdatedUserId = GetLoginUserId();
            int count = await database.UpdateAsync(emp);
            if(count > 0)
            {
                return updatedEmployee;
            }
            return null;
        }
    }
}
