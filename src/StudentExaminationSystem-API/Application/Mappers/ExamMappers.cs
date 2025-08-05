using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Application.Mappers;

public static class ExamMappers
{
    public static LoadExamAppDto MapToLoadExamAppDto(
        this GetFullExamInfraDto fullExam, ExamCacheEntryDto examCacheEntry)
    {
        return new LoadExamAppDto
        {
            Id = examCacheEntry.ExamId,
            SubjectId = examCacheEntry.SubjectId,
            ExamEndTime = examCacheEntry.ExamEndTime,
            Questions = fullExam.Questions.Select(qh => new LoadExamQuestionAppDto
            {
                Id = qh.Id,
                Content = qh.Content,
                Choices = qh.Choices.Select(c =>
                    c.MapTo<GetQuestionChoiceHistoryInfraDto, LoadExamChoiceAppDto>())
            }).ToList()
        };
    }
    
    public static void MapUpdate(this GeneratedExam examEntity, LoadExamAppDto submitExamDto)
    {
        foreach (var question in examEntity.QuestionHistory!)
        {
            var selectedChoice = submitExamDto.Questions
                .FirstOrDefault(q => q.Id == question.QuestionId)?
                .Choices.FirstOrDefault(c => c.IsSelected);
            if (selectedChoice != null)
                question.QuestionChoiceId = selectedChoice.Id;
        }
    }
}
