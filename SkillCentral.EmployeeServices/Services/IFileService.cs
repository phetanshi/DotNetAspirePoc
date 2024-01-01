using SkillCentral.Dtos;

namespace SkillCentral.EmployeeServices.Services
{
    public interface IFileService
    {
        Task<List<T>> ReadEmployeeBatchFile<T>(IFormFile formFile) where T : new();
    }
}
