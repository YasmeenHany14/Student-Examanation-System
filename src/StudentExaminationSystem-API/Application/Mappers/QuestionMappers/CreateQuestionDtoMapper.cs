using Application.DTOs.QuestionDtos;

namespace Application.Mappers.QuestionMappers;
using Domain.Models;

public static class CreateQuestionDtoMapper
{
    // to entity
    public static Question ToEntity(this CreateQuestionAppDto createQuestionDto)
    {
        return new Question
        {
            Content = createQuestionDto.Content,
            SubjectId = createQuestionDto.SubjectId,
            DifficultyId = createQuestionDto.DifficultyId,
            Choices = createQuestionDto.QuestionChoices?.Select(c => new QuestionChoice()
            {
                Content = c.Content,
                IsCorrect = c.IsCorrect
            }).ToList() ?? new List<QuestionChoice>(),
        };
    }
}
