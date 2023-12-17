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
        Task<EmployeeDto> Get(string userId);

        /// <summary>
        /// Retrieves all active employees in the system
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeDto>> Get();

        /// <summary>
        /// Creates an employee 
        /// </summary>
        /// <param name="employee">employee object of EmployeeCreateDto type</param>
        /// <returns></returns>
        Task<EmployeeDto> Create(EmployeeCreateDto employee);

        /// <summary>
        /// Updates employee details
        /// </summary>
        /// <param name="updatedEmployee"></param>
        /// <returns></returns>
        Task<EmployeeDto> Update(EmployeeDto updatedEmployee);

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="userId">Employee login user id which is a string</param>
        /// <returns></returns>
        Task<bool> Delete(string userId);

    }
}
