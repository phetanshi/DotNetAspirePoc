using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.SkillServices.Data.DbModels;

namespace SkillCentral.SkillServices.Utils
{
    public class SkillAutoMapperProfile : Profile
    {
        public SkillAutoMapperProfile()
        {
            CreateMap<Skill, SkillDto>();
            CreateMap<EmployeeSkill, EmployeeSkillDto>();

            CreateMap<SkillCreateDto, Skill>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedUserId, opt => opt.MapFrom(src => "admin"));

            CreateMap<EmployeeSkillCreateDto, EmployeeSkill>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedUserId, opt => opt.MapFrom(src => "admin"));
        }
    }
}
