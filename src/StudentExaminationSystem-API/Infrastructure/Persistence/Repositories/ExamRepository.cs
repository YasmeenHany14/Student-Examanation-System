using Domain.DTOs;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class ExamRepository(
    DataContext context,
    IPropertyMappingService propertyMappingService
    ) : BaseRepository<GeneratedExam>(context), IExamRepository 
{
    public async Task<PagedList<GetAllExamsInfraDto>> GetAllExamHistoryAsync(
        ExamHistoryResourceParameters resourceParameters, string? userId)
    {
        var collection = context.GeneratedExams.AsQueryable().AsNoTracking();
        var examMappingDictionary = propertyMappingService.GetPropertyMapping<GetAllExamsInfraDto, GeneratedExam>();
        
        if (!string.IsNullOrEmpty(userId))
            collection = collection.Where(e => e.Student.UserId == userId);

        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
        {
            var studentName = resourceParameters.SearchQuery.Trim().ToLower();
            collection = collection.Where(e => (e.Student.User.FirstName + " " + e.Student.User.LastName).ToLower().Contains(studentName));
        }
        
        var sortedList = collection.ApplySort(resourceParameters.OrderBy, examMappingDictionary);
        
        var projectedCollection = sortedList
            .Select(e => new GetAllExamsInfraDto
            {
                Id = e.Id,
                StudentName = e.Student.User.FirstName + " " + e.Student.User.LastName,
                SubjectName = e.Subject.Name,
                ExamStatus = (int)e.ExamStatus,
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
            .Where(e => e.ExamStatus == ExamStatus.Completed)
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
                UserId = e.Student.UserId,
                ExamStatus = e.ExamStatus,
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
                    Id = c.Id,
                    Content = c.Content,
                    IsCorrect = c.IsCorrect,
                    IsSelected = ah.QuestionChoiceId == c.Id
                })
            })
            .OrderBy(ah => ah.DisplayOrder)
            .ToListAsync();

        var questionHistory = answerHistoryQuery
            .Select(ah => new GetQuestionHistoryInfraDto
            {
                Id = ah.QuestionId,
                Content = ah.QuestionContent,
                Choices = ah.QuestionChoices.ToList(),
            })
            .ToList();
        
        return new GetFullExamInfraDto
        {
            userId = examInfo.UserId,
            FinalScore = examInfo.StudentScore,
            ExamStatus = examInfo.ExamStatus,
            Questions = questionHistory
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
