using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data.DbModels;

namespace SkillCentral.NotificationServices.Utils
{
    public class NotificationAutoMapperProfile : Profile
    {
        public NotificationAutoMapperProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationCreateDto, Notification>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => "admin"));
        }
    }
}
