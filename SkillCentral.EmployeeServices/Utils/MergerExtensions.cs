using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data.DbModels;

namespace SkillCentral.EmployeeServices.Utils
{
    public static class MergerExtensions
    {
        public static void MergerEmployee(this Employee emp, EmployeeDto updatedEmployee)
        {
            emp.FirstName = updatedEmployee.FirstName ?? emp.FirstName;
            emp.LastName = updatedEmployee.LastName ?? emp.LastName;
            emp.Designation = updatedEmployee.Designation ?? emp.Designation;
            emp.TotalExpInYears = updatedEmployee.TotalExpInYears > 0 ? updatedEmployee.TotalExpInYears : emp.TotalExpInYears;
            emp.TotalExpInMonths = updatedEmployee.TotalExpInMonths > 0 ? updatedEmployee.TotalExpInMonths : emp.TotalExpInMonths;
        }
    }
}
