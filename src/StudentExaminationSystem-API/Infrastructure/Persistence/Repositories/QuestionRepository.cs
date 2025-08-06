using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class QuestionRepository(
    DataContext context,
    IPropertyMappingService propertyMappingService
    ) : BaseRepository<Question>(context), IQuestionRepository
{
    public async Task<IEnumerable<CorrectAnswersInfraDto>> GetCorrectAnswersForQuestionsAsync(IEnumerable<int> questionIds)
    {
        var answersQuery = context.Questions
            .AsNoTracking()
            .Where(q => questionIds.Contains(q.Id))
            .AsQueryable();
        
        return await answersQuery
            .Select(q => new CorrectAnswersInfraDto
            {
                QuestionId = q.Id,
                CorrectAnswerId = q.Choices!
                    .Where(c => c.IsCorrect)
                    .Select(c => c.Id)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    public async Task<Question?> GetEntityByIdAsync(int id)
    {
        return await context.Questions
            .FindAsync(id);
    }
    public async Task<PagedList<GetQuestionInfraDto>> GetAllAsync(QuestionResourceParameters resourceParameters)
    {
        var collection = context.Questions
            .AsQueryable()
            .AsNoTracking()
            .Where(q => q.SubjectId == resourceParameters.SubjectId);
        
        var questionMappingDictionary = propertyMappingService
            .GetPropertyMapping<GetQuestionInfraDto, Question>();
        
        if (resourceParameters.DifficultyId.HasValue)
            collection = collection.Where(q => q.Difficulty == (Domain.Enums.Difficulty?)resourceParameters.DifficultyId);
        
        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
        {
            var searchQuery = resourceParameters.SearchQuery.Trim().ToLower();
            collection = collection.Where(q => q.Content.ToLower().StartsWith(searchQuery));
        }

        collection = collection.Include(q => q.Choices);
        
        var sortedList = collection.ApplySort(resourceParameters.OrderBy, questionMappingDictionary);
        var projectedCollection = sortedList.Select(q => new GetQuestionInfraDto
        {
            Id = q.Id,
            Content = q.Content,
            SubjectId = q.SubjectId,
            DifficultyId = (int?)q.Difficulty,
            IsActive = q.IsActive,
            Choices = q.Choices!.Select(c => new GetQuestionChoiceInfraDto
            {
                Id = c.Id,
                Content = c.Content,
                IsCorrect = c.IsCorrect
            }).ToList()
        });
        
        return await CreateAsync(
            projectedCollection,
            resourceParameters.PageNumber,
            resourceParameters.PageSize);
    }
    
    // public async Task<IEnumerable<LoadExamQuestionInfraDto>> GetQuestionsForExamAsync(
    //     GenerateExamConfigDto generateExamConfig)
    // {
    //     var allQuestions = new List<LoadExamQuestionInfraDto>();
    //     foreach (var difficulty in generateExamConfig.QuestionCounts)
    //     {
    //         var questions = await GetQuestionsByDifficultyAsync(
    //             (Difficulty)difficulty.Key, difficulty.Value);
    //         allQuestions.AddRange(questions);
    //     }
    //     return allQuestions.OrderBy(_ => Guid.NewGuid()).ToList();
    // }

    public async Task<IEnumerable<LoadExamQuestionInfraDto>> GetQuestionsForExamAsync(
        int subjectId,
        GenerateExamConfigDto generateExamConfig)
    {
        var allQuestions = new List<LoadExamQuestionInfraDto>();
        var maxQuestionsCount = generateExamConfig.QuestionCounts.Values.Max();
        var dbQuestions = await context.Questions
            .AsNoTracking()
            .Where(q => q.IsActive && q.SubjectId == subjectId)
            .Include(q => q.Choices)
            .OrderBy(q => Guid.NewGuid()) // Random ordering
            .Select(q => new LoadExamQuestionInfraDto
            {
                Id = q.Id,
                Content = q.Content,
                Difficulty = q.Difficulty,
                Choices = q.Choices!.Select(c => new LoadExamChoiceInfraDto
                {
                    ChoiceId = c.Id,
                    ChoiceText = c.Content,
                    IsSelected = false
                }).ToList()
            })
            .GroupBy(q => q.Difficulty)
            .Select(g => new
            {
                Key = g.Key,
                Questions = g.Take(maxQuestionsCount).ToList()
            })
            .ToListAsync();
        
        foreach (var difficulty in generateExamConfig.QuestionCounts)
        {
            var questions = dbQuestions
                .FirstOrDefault(g => (int)g.Key == difficulty.Key)?
                .Questions ?? new List<LoadExamQuestionInfraDto>();
            allQuestions.AddRange(questions);
        }
        return allQuestions.OrderBy(_ => Guid.NewGuid()).ToList();
    }

    private async Task<IEnumerable<LoadExamQuestionInfraDto>> GetQuestionsByDifficultyAsync(
        Difficulty difficulty, 
        int count)
    {
        if (count <= 0)
            return new List<LoadExamQuestionInfraDto>();

        return await context.Questions
            .AsNoTracking()
            .Where(q => q.Difficulty == difficulty && q.IsActive)
            .Include(q => q.Choices)
            .OrderBy(q => Guid.NewGuid()) // Random ordering
            .Take(count)
            .Select(q => new LoadExamQuestionInfraDto
            {
                Id = q.Id,
                Content = q.Content,
                Choices = q.Choices!.Select(c => new LoadExamChoiceInfraDto
                {
                    ChoiceId = c.Id,
                    ChoiceText = c.Content,
                    IsSelected = false
                }).ToList()
            })
            .ToListAsync();
    }
}
