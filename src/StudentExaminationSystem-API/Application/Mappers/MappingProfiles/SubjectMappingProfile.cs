using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Application.DTOs.SubjectsDtos;

namespace Application.Mappers.MappingProfiles;

public class SubjectMappingProfile : Profile
{
    public SubjectMappingProfile()
    {
        CreateMap<GetSubjectInfraDto, GetSubjectAppDto>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

        CreateMap<Subject, GetSubjectAppDto>()
            .ForMember(dest => dest.HasConfiguration, opt => opt.Ignore());

        CreateMap<CreateSubjectAppDto, Subject>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateSubjectAppDto, Subject>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}
