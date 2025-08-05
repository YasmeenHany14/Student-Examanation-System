using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers.MappingProfiles;

public class UserMappingProfiles : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<CreateUserAppDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}
