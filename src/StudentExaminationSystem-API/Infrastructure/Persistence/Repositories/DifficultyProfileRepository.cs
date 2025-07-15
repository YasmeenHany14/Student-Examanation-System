using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class DifficultyProfileRepository(
    DataContext context
    ) : BaseRepository<DifficultyProfile>(context), IDifficultyProfileRepository
{
    public async Task<IEnumerable<GetDifficultyProfileInfraDto>> GetAllAsync()
    {
        return await context.DifficultyProfiles
            .AsNoTracking()
            .Select(s => new GetDifficultyProfileInfraDto
            {
                Id = s.Id,
                Name = s.Name + " (" + s.EasyPercentage + "% Easy, " + s.MediumPercentage + "% Medium, " + s.HardPercentage + "% Hard)",
            })
            .ToListAsync();
    }

    public async Task<PagedList<GetDifficultyProfileInfraDto>> GetAllAsync(
        DifficultyProfileResourceParameters resourceParameters)
    {
        var collection = context.DifficultyProfiles.AsNoTracking();
             
        var projectedCollection = collection.Select(s => new GetDifficultyProfileInfraDto
        {
            Id = s.Id,
            Name = s.Name,
            EasyQuestionsPercent = s.EasyPercentage,
            MediumQuestionsPercent = s.MediumPercentage,
            HardQuestionsPercent = s.HardPercentage
        });
        
        // var sortedList = sortHelper.ApplySort(collection, resourceParameters.OrderBy);
        var createdCollection = await CreateAsync(
            projectedCollection,
            resourceParameters.PageNumber,
            resourceParameters.PageSize);
        return createdCollection;
    }

    public async Task<GetDifficultyProfileInfraDto?> GetByIdAsync(int id)
    {
        return await context.DifficultyProfiles
            .Where(s => s.Id == id)
            .Select(s => new GetDifficultyProfileInfraDto
            {
                Id = s.Id,
                Name = s.Name,
                EasyQuestionsPercent = s.EasyPercentage,
                MediumQuestionsPercent = s.MediumPercentage,
                HardQuestionsPercent = s.HardPercentage
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DifficultyProfile?> GetEntityByIdAsync(int id)
    {
        return await context.DifficultyProfiles
            .FindAsync(id);
    }
}
