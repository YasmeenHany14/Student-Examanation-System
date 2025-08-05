using AutoMapper;
using Application.DTOs.DifficultyProfileDtos;
using Domain.DTOs;
using Domain.Models;
using Shared.ResourceParameters;

namespace Application.Mappers.MappingProfiles;

public class DifficultyProfileMappingProfiles : Profile
{
    public DifficultyProfileMappingProfiles()
    {
        // GetDifficultyProfileInfraDto to GetDifficultyProfileAppDto
        CreateMap<GetDifficultyProfileInfraDto, GetDifficultyProfileAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // CreateUpdateDifficultyProfileAppDto to DifficultyProfile entity
        CreateMap<CreateUpdateDifficultyProfileAppDto, DifficultyProfile>()
            .ForMember(dest => dest.EasyPercentage, opt => opt.MapFrom(src => src.EasyQuestionsPercent))
            .ForMember(dest => dest.MediumPercentage, opt => opt.MapFrom(src => src.MediumQuestionsPercent))
            .ForMember(dest => dest.HardPercentage, opt => opt.MapFrom(src => src.HardQuestionsPercent))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // Update mapping for existing entity
        CreateMap<CreateUpdateDifficultyProfileAppDto, DifficultyProfile>()
            .ForMember(dest => dest.EasyPercentage, opt => opt.MapFrom(src => src.EasyQuestionsPercent))
            .ForMember(dest => dest.MediumPercentage, opt => opt.MapFrom(src => src.MediumQuestionsPercent))
            .ForMember(dest => dest.HardPercentage, opt => opt.MapFrom(src => src.HardQuestionsPercent))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // PagedList mapping
        CreateMap<PagedList<GetDifficultyProfileInfraDto>, PagedList<GetDifficultyProfileAppDto>>()
            .ConvertUsing((src, dest, context) => 
                new PagedList<GetDifficultyProfileAppDto>(
                    src.Pagination,
                    context.Mapper.Map<List<GetDifficultyProfileAppDto>>(src.Data)
                ));
    }
}
