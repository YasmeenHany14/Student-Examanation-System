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
    
    public async Task<AdminDashboardInfraDto> GetDashboardDataAsync()
    {
        var query = await context.GeneratedExams
            .AsNoTracking()
            .Where(e => e.IsCompleted)
            .Select(e => new { passed = e.StudentScore >= e.ExamTotalScore / 2, })
            .ToListAsync();
        
        return new AdminDashboardInfraDto
        {
            TotalExamsCompleted = query.Count,
            PassedExamsCount = query.Count(e => e.passed),
            FailedExamsCount = query.Count(e => !e.passed),
        };
    }
    
    public async Task<GetFullExamInfraDto?> GetAllQuestionHistoryAsync(int examId)
    {
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

        var answerHistoryQuery = await context.AnswerHistories
            .AsNoTracking()
            .Where(ah => ah.GeneratedExamId == examId)
            .Select(ah => new
            {
                ah.QuestionId,
                ah.DisplayOrder,
                QuestionContent = ah.Question.Content,
                QuestionChoices = ah.Question.Choices.Select(c => new GetQuestionChoiceHistoryInfraDto
                {
                    ChoiceId = c.Id,
                    ChoiceText = c.Content,
                    IsCorrect = c.IsCorrect
                })
            })
            .OrderBy(ah => ah.DisplayOrder)
            .ToListAsync();

        var questionHistory = answerHistoryQuery
            .GroupBy(ah => ah.QuestionId)
            .Select(g => new GetQuestionHistoryInfraDto
            {
                QuestionId = g.Key,
                Question = g.First().QuestionContent ?? string.Empty,
                Choices = g.First().QuestionChoices ?? new List<GetQuestionChoiceHistoryInfraDto>(),
            })
            .ToList();

        return new GetFullExamInfraDto
        {
            userId = examInfo.UserId,
            FinalScore = examInfo.StudentScore,
            QuestionHistory = questionHistory
        };
    }

    public async Task<GeneratedExam?> GetExamForUpdate(int examId)
    {
        return await context.GeneratedExams
            .Include(e => e.QuestionHistory)
            .FirstOrDefaultAsync(e => e.Id == examId);
    }

    public async Task<bool> ExamExistsAsync(int userId, int subjectId)
    {
        return await context.GeneratedExams
            .AsNoTracking()
            .AnyAsync(e => e.StudentId == userId && e.SubjectId == subjectId);
    }
}
