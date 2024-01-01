using SkillCentral.Dtos;

namespace SkillCentral.EmployeeServices.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves employee details whose user ID matches the given user ID
        /// </summary>
        /// <param name="userId">Employee login user id which is a string</param>
        /// <returns></returns>
        Task<EmployeeDto> GetAsync(string userId);

        /// <summary>
        /// Retrieves all active employees in the system
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeDto>> GetAsync();

        /// <summary>
        /// Creates an employee 
        /// </summary>
        /// <param name="employee">employee object of EmployeeCreateDto type</param>
        /// <returns></returns>
        Task<EmployeeDto> CreateAsync(EmployeeCreateDto employee, bool isBatch = false);

        /// <summary>
        /// Updates employee details
        /// </summary>
        /// <param name="updatedEmployee"></param>
        /// <returns></returns>
        Task<EmployeeDto> UpdateAsync(EmployeeDto updatedEmployee, bool isBatch = false);

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="userId">Employee login user id which is a string</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string userId, bool isBatch = false);

        /// <summary>
        /// Creates list of employee profiles 
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        Task ProcessEmployeeBatchAsync(List<EmployeeDto> employees);

    }
}
