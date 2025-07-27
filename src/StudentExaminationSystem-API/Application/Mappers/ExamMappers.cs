using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Application.Mappers;

public static class ExamMappers
{
    public static PagedList<GetExamHistoryAppDto> ToListDto(this PagedList<GetAllExamsInfraDto> exams)
    {
        return new PagedList<GetExamHistoryAppDto>(
            exams.Pagination,
            exams.Data.Select(s => s.MapTo<GetAllExamsInfraDto, GetExamHistoryAppDto>()).ToList()
        );
    }

    private static GetQuestionHistoryAppDto MapToGetQuestionHistoryAppDto(
        this GetQuestionHistoryInfraDto questionHistory)
    {
        return new GetQuestionHistoryAppDto
        {
            Content = questionHistory.Content,
            Choices = questionHistory.Choices.Select(c =>
                c.MapTo<GetQuestionChoiceHistoryInfraDto, GetQuestionChoiceHistoryAppDto>())
        };
    }
    
    public static GetFullExamAppDto MapToGetFullExamAppDto(
        this GetFullExamInfraDto fullExam)
    {
        return new GetFullExamAppDto
        {
            FinalScore = fullExam.FinalScore,
            Passed = fullExam.FinalScore >= (fullExam.Questions.Count() / 2),
            QuestionHistory = fullExam.Questions.Select(qh => qh.MapToGetQuestionHistoryAppDto())
        };
    }

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
    
    public static void MapUpdate(this GeneratedExam examEntity, SubmitExamAppDto submitExamDto)
    {
        foreach (var answer in submitExamDto.Answers)
        {
            var existingQuestionHistory = examEntity.QuestionHistory?
                .FirstOrDefault(qh => qh.QuestionId == answer.QuestionId);
                
            if (existingQuestionHistory != null)
            {
                existingQuestionHistory.QuestionChoiceId = answer.ChoiceId;
                existingQuestionHistory.IsCorrect = answer.IsCorrect;
            }
        }
    }
}
