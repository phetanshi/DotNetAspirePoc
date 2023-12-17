using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data;
using SkillCentral.EmployeeServices.Data.DbModels;

namespace SkillCentral.EmployeeServices.Services
{
    public class EmployeeService(EmployeeDbContext database, IMapper mapper, ILogger<EmployeeService> logger) : IEmployeeService
    {
        public async Task<EmployeeDto> Create(EmployeeCreateDto employee)
        {
            if (employee is null)
                throw new ArgumentNullException("employee input object cannot be null");

            var dbEmp = mapper.Map<Employee>(employee);

            database.Employees.Add(dbEmp);
            await database.SaveChangesAsync();

            return mapper.Map<EmployeeDto>(dbEmp);
        }

        public async Task<bool> Delete(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException("userId cannot be null");

            var dbEmp = await database.Employees.FirstOrDefaultAsync(s => string.Equals(s.UserId, userId, StringComparison.OrdinalIgnoreCase));
            dbEmp.IsActive = false;
            int count = await database.SaveChangesAsync();
            return count > 0;
        }

        public async Task<EmployeeDto> Get(string userId)
        {
            var emp = await database.Employees.FirstOrDefaultAsync(x => string.Equals(x.UserId, userId, StringComparison.OrdinalIgnoreCase));
            if (emp is not null)
            {
                return mapper.Map<EmployeeDto>(emp);
            }
            return null;
        }

        public async Task<List<EmployeeDto>> Get()
        {
            var lst = await database.Employees.Where(x => x.IsActive).ToListAsync();
            if(lst is not null && lst.Count > 0)
            {
                return mapper.Map<List<EmployeeDto>>(lst);
            }
            return new List<EmployeeDto>();
        }

        public async Task<EmployeeDto> Update(EmployeeDto updatedEmployee)
        {

            if (updatedEmployee is null)
                throw new ArgumentNullException("employee input object cannot be null");

            var emp = await database.Employees.FirstOrDefaultAsync(x => string.Equals(x.UserId, updatedEmployee.UserId, StringComparison.OrdinalIgnoreCase));
            mapper.Map(updatedEmployee, emp);
            int count = await database.SaveChangesAsync();
            if(count > 0)
            {
                return updatedEmployee;
            }
            return null;
        }
    }
}
