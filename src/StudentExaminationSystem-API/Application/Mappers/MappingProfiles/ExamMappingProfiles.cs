using AutoMapper;
using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Domain.DTOs;

namespace Application.Mappers.MappingProfiles;

public class ExamMappingProfiles : Profile
{
    public ExamMappingProfiles()
    {
        // GetAllExamsInfraDto to GetExamHistoryAppDto
        CreateMap<GetAllExamsInfraDto, GetExamHistoryAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // GetQuestionHistoryInfraDto to GetQuestionHistoryAppDto
        CreateMap<GetQuestionHistoryInfraDto, GetQuestionHistoryAppDto>()
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        // GetQuestionChoiceHistoryInfraDto to GetQuestionChoiceHistoryAppDto
        CreateMap<GetQuestionChoiceHistoryInfraDto, GetQuestionChoiceHistoryAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // GetFullExamInfraDto to GetFullExamAppDto
        CreateMap<GetFullExamInfraDto, GetFullExamAppDto>()
            .ForMember(dest => dest.FinalScore, opt => opt.MapFrom(src => src.FinalScore))
            .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.FinalScore >= (int)Math.Ceiling(src.Questions.Count() / 2.0)))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        // GetFullExamInfraDto to LoadExamAppDto (requires custom mapping with ExamCacheEntryDto)
        CreateMap<GetFullExamInfraDto, LoadExamAppDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Will be set from ExamCacheEntryDto
            .ForMember(dest => dest.SubjectId, opt => opt.Ignore()) // Will be set from ExamCacheEntryDto
            .ForMember(dest => dest.ExamEndTime, opt => opt.Ignore()) // Will be set from ExamCacheEntryDto
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        // GetQuestionHistoryInfraDto to LoadExamQuestionAppDto
        CreateMap<GetQuestionHistoryInfraDto, LoadExamQuestionAppDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.QuestionOrder, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        // GetQuestionChoiceHistoryInfraDto to LoadExamChoiceAppDto
        CreateMap<GetQuestionChoiceHistoryInfraDto, LoadExamChoiceAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // ExamCacheEntryDto mappings
        CreateMap<ExamCacheEntryDto, LoadExamAppDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExamId))
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
            .ForMember(dest => dest.ExamEndTime, opt => opt.MapFrom(src => src.ExamEndTime))
            .ForMember(dest => dest.Questions, opt => opt.Ignore()); // Will be mapped separately
    }
}
