using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data.DbModels;

namespace SkillCentral.NotificationServices.Utils
{
    public class NotificationAutoMapperProfile : Profile
    {
        public NotificationAutoMapperProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Notification, opt => opt.MapFrom(src => src.Message))
                .ReverseMap();

            CreateMap<NotificationCreateDto, Notification>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Notification))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedUserId, opt => opt.MapFrom(src => "admin"));
        }
    }
}
