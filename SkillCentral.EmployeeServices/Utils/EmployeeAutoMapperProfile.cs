using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data.DbModels;

namespace SkillCentral.EmployeeServices.Utils
{
    public class EmployeeAutoMapperProfile : Profile
    {
        public EmployeeAutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ReverseMap();

            CreateMap<EmployeeCreateDto, Employee>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedUserId, opt => opt.MapFrom(src => "admin"));
        }
    }
}
