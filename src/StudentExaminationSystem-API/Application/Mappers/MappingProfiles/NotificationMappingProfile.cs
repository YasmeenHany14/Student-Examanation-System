using AutoMapper;

namespace Application.Mappers.MappingProfiles;

public class NotificationMappingProfile : Profile
{
    // from entity to DTO
    public NotificationMappingProfile()
    {
        CreateMap<Domain.Models.Notification, Application.DTOs.NotificationAppDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead))
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.CreatedAt));
    }
}