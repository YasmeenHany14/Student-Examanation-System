using Application.DTOs.QuestionDtos;
using Domain.Enums;

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
            Difficulty = (Difficulty?)createQuestionDto.DifficultyId,
            IsActive = true,
            Choices = createQuestionDto.QuestionChoices?.Select(c => new QuestionChoice()
            {
                Content = c.Content,
                IsCorrect = c.IsCorrect
            }).ToList() ?? new List<QuestionChoice>(),
        };
    }
}
