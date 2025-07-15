using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class QuestionRepository(DataContext context) : BaseRepository<Question>(context), IQuestionRepository
{
    public async Task<PagedList<GetQuestionInfraDto>> GetAllAsync(QuestionResourceParameters resourceParameters)
    {
        var collection = context.Questions
            .AsQueryable()
            .AsNoTracking()
            .Where(q => q.SubjectId == resourceParameters.SubjectId);
        
        if (resourceParameters.DifficultyId.HasValue)
            collection = collection.Where(q => q.Difficulty == (Domain.Enums.Difficulty?)resourceParameters.DifficultyId);

        collection = collection.Include(q => q.Choices);
        var projectedCollection = collection.Select(q => new GetQuestionInfraDto
        {
            Id = q.Id,
            Content = q.Content,
            SubjectId = q.SubjectId,
            DifficultyId = (int?)q.Difficulty,
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
}
