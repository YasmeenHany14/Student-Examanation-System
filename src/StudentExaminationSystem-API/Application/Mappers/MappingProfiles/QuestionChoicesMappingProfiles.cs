using AutoMapper;
using Application.DTOs.QuestionChoiceDtos;
using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;

namespace Application.Mappers.MappingProfiles;

public class QuestionChoicesMappingProfiles : Profile
{
    public QuestionChoicesMappingProfiles()
    {
        // CreateQuestionChoiceAppDto to QuestionChoice entity
        CreateMap<CreateQuestionChoiceAppDto, QuestionChoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.QuestionId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.AnswerHistories, opt => opt.Ignore())
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // GetQuestionChoiceInfraDto to GetQuestionChoiceAppDto
        CreateMap<GetQuestionChoiceInfraDto, GetQuestionChoiceAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // LoadExamChoiceInfraDto to LoadExamChoiceAppDto
        CreateMap<LoadExamChoiceInfraDto, LoadExamChoiceAppDto>();
    }
}
