using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class DifficultyProfileRepository(
    DataContext context,
    IPropertyMappingService propertyMappingService
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
        var propertyMappingDictionary = propertyMappingService
            .GetPropertyMapping<GetDifficultyProfileInfraDto, DifficultyProfile>();
        
        if (!string.IsNullOrWhiteSpace(resourceParameters.Name))
        {
            var name = resourceParameters.Name.Trim().ToLower();
            collection = collection.Where(s => s.Name.ToLower().Contains(name));
        }
        
        var sortedList = collection.ApplySort(resourceParameters.OrderBy, propertyMappingDictionary);
        var projectedCollection = sortedList.Select(s => new GetDifficultyProfileInfraDto
        {
            Id = s.Id,
            Name = s.Name,
            EasyQuestionsPercent = s.EasyPercentage,
            MediumQuestionsPercent = s.MediumPercentage,
            HardQuestionsPercent = s.HardPercentage
        });
        
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
