using Application.DTOs.QuestionChoiceDtos;
using AutoMapper;
using Application.DTOs.QuestionDtos;
using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;
using Domain.Enums;

namespace Application.Mappers.MappingProfiles;

public class QuestionMappingProfiles : Profile
{
    public QuestionMappingProfiles()
    {
        // CreateQuestionAppDto to Question entity
        CreateMap<CreateQuestionAppDto, Question>()
            .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => (Difficulty?)src.DifficultyId))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices))
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));

        // GetQuestionInfraDto to GetQuestionAppDto
        CreateMap<GetQuestionInfraDto, GetQuestionAppDto>()
            .ForAllMembers(dest => dest.Condition((src, dest, srcMember) => srcMember != null));


        // LoadExamQuestionInfraDto to LoadExamQuestionAppDto
        CreateMap<LoadExamQuestionInfraDto, LoadExamQuestionAppDto>()
            .ForMember(dest => dest.QuestionOrder, opt => opt.Ignore())
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        // Collection mapping for LoadExamQuestionInfraDto to LoadExamQuestionAppDto with question order
        CreateMap<IEnumerable<LoadExamQuestionInfraDto>, IEnumerable<LoadExamQuestionAppDto>>()
            .ConvertUsing((src, dest, context) =>
            {
                var result = new List<LoadExamQuestionAppDto>();
                var questionOrder = 1;
                foreach (var question in src)
                {
                    var mappedQuestion = context.Mapper.Map<LoadExamQuestionAppDto>(question);
                    mappedQuestion.QuestionOrder = questionOrder++;
                    result.Add(mappedQuestion);
                }
                return result;
            });
    }
}
