using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Application.DTOs.SubjectExamConfigDtos;

namespace Application.Mappers.MappingProfiles;

public class SubjectConfigMappingProfile : Profile
{
    public SubjectConfigMappingProfile()
    {
        // GetSubjectExamConfigInfraDto to GetSubjectExamConfigAppDto
        CreateMap<GetSubjectExamConfigInfraDto, GetSubjectExamConfigAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // CreateUpdateSubjectExamConfig to SubjectExamConfig (handles both create and update)
        CreateMap<CreateUpdateSubjectExamConfig, SubjectExamConfig>()
            .ForMember(dest => dest.SubjectId, opt => opt.Ignore())
            .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes!.Value))
            .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.TotalQuestions!.Value))
            .ForMember(dest => dest.DifficultyProfileId, opt => opt.MapFrom(src => src.DifficultyProfileId!.Value));
    }
}
