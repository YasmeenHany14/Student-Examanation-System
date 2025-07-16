using Application.DTOs.SubjectsDtos;
using AutoMapper;
using Domain.DTOs;

namespace Application.Mappers.SubjectMappers;

public class SubjectMappingProfiles : Profile
{
    public SubjectMappingProfiles()
    {
        CreateMap<GetSubjectInfraDto, GetSubjectAppDto>()
            .ForMember(dest => dest.HasConfiguration, opt => opt.MapFrom(src => src.HasConfiguration != null))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code != null));
    }
}
