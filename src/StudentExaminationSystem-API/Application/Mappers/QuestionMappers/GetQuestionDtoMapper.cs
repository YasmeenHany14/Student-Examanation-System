using Application.DTOs.QuestionChoiceDtos;
using Application.DTOs.QuestionDtos;
using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Application.Mappers.QuestionMappers;

public static class GetQuestionDtoMapper
{
    public static GetQuestionAppDto ToGetQuestionAppDto(this GetQuestionInfraDto questionDto)
    {
        return new GetQuestionAppDto
        {
            Id = questionDto.Id,
            Content = questionDto.Content,
            SubjectId = questionDto.SubjectId,
            DifficultyId = questionDto.DifficultyId,
            IsActive = questionDto.IsActive,
            Choices = questionDto.Choices.Select(c => new GetQuestionChoiceAppDto
            {
                Id = c.Id,
                Content = c.Content,
                IsCorrect = c.IsCorrect
            }).ToList()
        };
    }
    
    public static PagedList<GetQuestionAppDto> ToListDto(
        this PagedList<GetQuestionInfraDto> questions)
    {
        return new PagedList<GetQuestionAppDto> (
            questions.Pagination,
            questions.Data.Select(s => ToGetQuestionAppDto(s)).ToList());
    }
    
    public static IEnumerable<LoadExamQuestionAppDto> ToLoadExamQuestionAppDto(
        this IEnumerable<LoadExamQuestionInfraDto> questions)
    {
        return questions.Select((q, index) => new LoadExamQuestionAppDto
        {
            QuestionId = q.QuestionId,
            QuestionText = q.QuestionText,
            QuestionOrder = index + 1,
            Choices = q.Choices.Select(c => c.MapTo<LoadExamChoiceInfraDto, LoadExamChoiceAppDto>())
        }).ToList();
    }
}
