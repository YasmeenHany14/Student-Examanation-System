using System.Linq.Dynamic.Core;
using Domain.DTOs.SubjectExamConfigDtos;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SubjectExamConfigRepository(DataContext context)
    : BaseRepository<SubjectExamConfig>(context), ISubjectExamConfigRepository
{
    public async Task<GetSubjectExamConfigInfraDto?> GetByIdAsync(int id)
    {
        return await context.SubjectExamConfigs
            .Select(config => new GetSubjectExamConfigInfraDto
            {
                Id = config.SubjectId,
                DurationMinutes = config.DurationMinutes,
                TotalQuestions = config.TotalQuestions,
                DifficultyProfileId = config.DifficultyProfileId,
                DifficultyProfileSpecifications =
                    config.DifficultyProfile.Name + " " +
                    config.DifficultyProfile.EasyPercentage + "% Easy, " +
                    config.DifficultyProfile.MediumPercentage + "% Medium, " +
                    config.DifficultyProfile.HardPercentage + "% Hard"
            }).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.SubjectExamConfigs.AnyAsync(c => c.SubjectId == id);
    }
    
    public async Task<SubjectExamConfig?> FindAsync(int id)
    {
        return await context.SubjectExamConfigs.FindAsync(id);
    }
}
