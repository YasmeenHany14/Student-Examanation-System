using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class ExamRepository(
    DataContext context
    ) : BaseRepository<GeneratedExam>(context), IExamRepository 
{
    public async Task<PagedList<GetAllExamsInfraDto>> GetAllExamHistoryAsync(
        ExamHistoryResourceParameters resourceParameters, string? userId)
    {
        var collection = context.GeneratedExams.AsQueryable().AsNoTracking();
        if (!string.IsNullOrEmpty(userId))
            collection = collection.Where(e => e.Student.UserId == userId);
        
        var projectedCollection = collection
            .Select(e => new GetAllExamsInfraDto
            {
                Id = e.Id,
                StudentName = e.Student.User.FirstName + " " + e.Student.User.LastName,
                SubjectName = e.Subject.Name,
                ExamDate = e.CreatedAt,
                FinalScore = e.StudentScore,
                Passed = e.StudentScore >= e.ExamTotalScore/2,
            });
        return await CreateAsync(
            projectedCollection,
            resourceParameters.PageNumber,
            resourceParameters.PageSize);
    }
    
    public async Task<GetFullExamInfraDto?> GetAllQuestionHistoryAsync(int examId)
    {
        // First, get basic exam info and validate existence
        var examInfo = await context.GeneratedExams
            .AsNoTracking()
            .Where(e => e.Id == examId)
            .Select(e => new 
            {
                e.StudentScore,
                UserId = e.Student.UserId
            })
            .FirstOrDefaultAsync();

        if (examInfo == null)
            return null;

        // Get question history with minimal data - more efficient query
        var answerHistoryQuery = await context.AnswerHistories
            .AsNoTracking()
            .Where(ah => ah.GeneratedExamId == examId)
            .Select(ah => new
            {
                ah.QuestionId,
                ah.DisplayOrder,
                QuestionContent = ah.Question.Content,
                QuestionChoices = ah.Question.Choices.Select(c => new GetQuestionChoiceInfraDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    IsCorrect = c.IsCorrect
                })
            })
            .OrderBy(ah => ah.DisplayOrder)
            .ToListAsync();

        // Group and project to DTOs - done in memory but with minimal data
        var questionHistory = answerHistoryQuery
            .GroupBy(ah => ah.QuestionId)
            .Select(g => new GetQuestionHistoryInfraDto
            {
                Question = g.First().QuestionContent ?? string.Empty,
                Choices = g.First().QuestionChoices ?? new List<GetQuestionChoiceInfraDto>()
            })
            .ToList();

        return new GetFullExamInfraDto
        {
            userId = examInfo.UserId,
            FinalScore = examInfo.StudentScore,
            QuestionHistory = questionHistory
        };
    }
}
