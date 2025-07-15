using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Domain.DTOs;
using Shared.ResourceParameters;

namespace Application.Mappers;

public static class ExamMappers
{
    public static PagedList<GetExamHistoryAppDto> ToListDto(this PagedList<GetAllExamsInfraDto> exams)
    {
        var count = exams.TotalCount;
        var pageNumber = exams.CurrentPage;
        var pageSize = exams.PageSize;
        var totalPages = exams.TotalPages;
        return new PagedList<GetExamHistoryAppDto>(
            exams.Select(s => s.MapTo<GetAllExamsInfraDto, GetExamHistoryAppDto>()).ToList(),
            count,
            pageNumber,
            pageSize,
            totalPages
        );
    }

    private static GetQuestionHistoryAppDto MapToGetQuestionHistoryAppDto(
        this GetQuestionHistoryInfraDto questionHistory)
    {
        return new GetQuestionHistoryAppDto
        {
            Question = questionHistory.Question,
            Choices = questionHistory.Choices.Select(c =>
                c.MapTo<GetQuestionChoiceInfraDto, GetQuestionChoiceHistoryAppDto>())
        };
    }
    
    public static GetFullExamAppDto MapToGetFullExamAppDto(
        this GetFullExamInfraDto fullExam)
    {
        return new GetFullExamAppDto
        {
            FinalScore = fullExam.FinalScore,
            Passed = fullExam.FinalScore >= (fullExam.QuestionHistory.Count() / 2),
            QuestionHistory = fullExam.QuestionHistory.Select(qh => qh.MapToGetQuestionHistoryAppDto())
        };
    }
}
