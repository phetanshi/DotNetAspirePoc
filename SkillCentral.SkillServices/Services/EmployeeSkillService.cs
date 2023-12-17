using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Data;

namespace SkillCentral.SkillServices.Services
{
    public class EmployeeSkillService(SkillDbContext database, IMapper mapper, ILogger<EmployeeSkillService> logger) : IEmployeeSkillService
    {
        public Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill)
        {
            throw new NotImplementedException();
        }

        public Task<List<EmployeeSkillDto>> GetAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
