using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class SubjectRepository(DataContext context) : BaseRepository<Subject>(context), ISubjectRepository
{
    public async Task<bool> CheckSubjectsExists(IEnumerable<int> subjectIds)
    {
        return await context.Subjects
            .Where(s => subjectIds.Contains(s.Id))
            .CountAsync() == subjectIds.Count();
    }

    public async Task<bool> CheckCodeUniqueAsync(string code, int? id = null)
    {
        var query = context.Subjects.AsNoTracking().Where(s => s.Code == code);
        
        if (id.HasValue)
            query = query.Where(s => s.Id != id.Value);
        
        return await query.AnyAsync();
    }

    public async Task<PagedList<GetSubjectInfraDto>> GetAllAsync(SubjectResourceParameters resourceParameters)
    {
        var collection = context.Subjects.AsQueryable().AsNoTracking();
        var projectedCollection = collection
            .Select(s => new GetSubjectInfraDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                HasConfiguration = s.SubjectExamConfig != null
            });
        
        //TODO: add name, code filters later

        return await CreateAsync(projectedCollection, resourceParameters.PageNumber, resourceParameters.PageSize);
    }

    public async Task<IEnumerable<GetSubjectInfraDto>> GetAllAsync(string? userId = null)
    {
        var query = await context.Subjects
            .AsNoTracking()
            .AsQueryable()
            .Select(s => new GetSubjectInfraDto
            {
                Id = s.Id,
                Name = s.Name + " (" + s.Code + ")",
            })
            .ToListAsync();
        return userId == null ? query : query.Where(s => s.Id.ToString() == userId);
    }

    public async Task<GetSubjectInfraDto?> GetByIdAsync(int id)
    {
        return await context.Subjects
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new GetSubjectInfraDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                HasConfiguration = s.SubjectExamConfig != null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Subject?> GetEntityByIdAsync(int id)
    {
        return await context.Subjects
            .FindAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Subjects
            .AsNoTracking()
            .AnyAsync(s => s.Id == id);
    }
}
